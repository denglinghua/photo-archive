package config

import (
	"encoding/json"
	"fmt"
	"os"
	"path/filepath"
	"strings"

	"photo-archive/internal/fsutil"
)

const FileName = "photo-archive.json"

// Config holds runtime settings loaded from photo-archive.json.
type Config struct {
	SourceDir       string   `json:"source_dir"`
	DestDir         string   `json:"dest_dir"`
	VideoDestDir    string   `json:"video_dest_dir"`
	ImageExtensions []string `json:"image_extensions"`
	VideoExtensions []string `json:"video_extensions"`
	DeletePatterns  []string `json:"delete_patterns"`
}

// PathBesideExecutable returns <dir-of-binary>/photo-archive.json.
func PathBesideExecutable() (string, error) {
	exe, err := os.Executable()
	if err != nil {
		return "", err
	}
	resolved, err := filepath.EvalSymlinks(exe)
	if err != nil {
		resolved = exe
	}
	return filepath.Join(filepath.Dir(resolved), FileName), nil
}

// Load reads and validates a config JSON file.
func Load(path string) (Config, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return Config{}, fmt.Errorf("read config '%s': %w", path, err)
	}

	var cfg Config
	if err := json.Unmarshal(data, &cfg); err != nil {
		return Config{}, fmt.Errorf("parse config '%s': %w", path, err)
	}

	cfg.SourceDir = strings.TrimSpace(cfg.SourceDir)
	cfg.DestDir = strings.TrimSpace(cfg.DestDir)
	cfg.VideoDestDir = strings.TrimSpace(cfg.VideoDestDir)
	cfg.ImageExtensions = normalizeExts(cfg.ImageExtensions)
	cfg.VideoExtensions = normalizeExts(cfg.VideoExtensions)

	if cfg.SourceDir == "" {
		return Config{}, fmt.Errorf("config: source_dir is required")
	}
	if cfg.DestDir == "" {
		return Config{}, fmt.Errorf("config: dest_dir is required")
	}
	if cfg.VideoDestDir == "" {
		return Config{}, fmt.Errorf("config: video_dest_dir is required")
	}
	if len(cfg.ImageExtensions) == 0 && len(cfg.VideoExtensions) == 0 {
		return Config{}, fmt.Errorf("config: image_extensions or video_extensions required")
	}

	return cfg, nil
}

// ExampleJSON is printed when config is missing or invalid.
const ExampleJSON = `{
  "source_dir": "/path/to/inbox",
  "dest_dir": "/path/to/photo-archive",
  "video_dest_dir": "/path/to/video-archive",
  "image_extensions": ["jpg", "jpeg", "png", "heic"],
  "video_extensions": ["mov", "avi", "mp4", "m4v"],
  "delete_patterns": ["*.AAE"]
}`

// IsImage reports whether ext (lowercase, no dot) is configured as an image.
func (c Config) IsImage(ext string) bool {
	ext = strings.ToLower(ext)
	for _, e := range c.ImageExtensions {
		if e == ext {
			return true
		}
	}
	return false
}

// IsVideo reports whether ext (lowercase, no dot) is configured as a video.
func (c Config) IsVideo(ext string) bool {
	ext = strings.ToLower(ext)
	for _, e := range c.VideoExtensions {
		if e == ext {
			return true
		}
	}
	return false
}

// AcceptedExtensions returns the union of image and video extensions.
func (c Config) AcceptedExtensions() map[string]struct{} {
	return fsutil.ExtensionSet(c.ImageExtensions, c.VideoExtensions)
}

func normalizeExts(exts []string) []string {
	out := make([]string, 0, len(exts))
	for _, e := range exts {
		e = strings.ToLower(strings.TrimSpace(strings.TrimPrefix(e, ".")))
		if e == "" {
			continue
		}
		out = append(out, e)
	}
	return out
}
