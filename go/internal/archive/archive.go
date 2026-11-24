package archive

import (
	"fmt"
	"os"
	"path/filepath"
	"time"

	"photo-archive/internal/config"
	"photo-archive/internal/fsutil"
)

// Stats holds archive counters.
type Stats struct {
	Total   int
	Moved   int
	Exist   int
	Ignored int
	Error   int
}

func (s Stats) String() string {
	return fmt.Sprintf(" total=%d\n moved=%d\n exist=%d\n ignored=%d\n error=%d",
		s.Total, s.Moved, s.Exist, s.Ignored, s.Error)
}

// Run moves renamed media from cfg.SourceDir into photo/video dest roots.
// Photos → dest_dir/yyyy/yyyyMM/; videos → video_dest_dir/yyyy/.
func Run(cfg config.Config) (Stats, error) {
	files, err := fsutil.ListFiles(cfg.SourceDir, cfg.AcceptedExtensions())
	if err != nil {
		return Stats{}, err
	}

	stats := Stats{Total: len(files)}
	for _, path := range files {
		if err := moveOne(path, cfg, &stats); err != nil {
			fmt.Fprintf(os.Stderr, "archive '%s' error: %v\n", filepath.Base(path), err)
			stats.Error++
		}
	}
	return stats, nil
}

func moveOne(path string, cfg config.Config, stats *Stats) error {
	name := filepath.Base(path)
	monthKey := extractTakenMonth(name)
	if monthKey == "" {
		stats.Ignored++
		return nil
	}
	year := monthKey[:4]
	ext := fsutil.ExtLower(name)

	var archiveDir string
	if cfg.IsVideo(ext) {
		archiveDir = filepath.Join(cfg.VideoDestDir, year)
	} else {
		archiveDir = filepath.Join(cfg.DestDir, year, monthKey)
	}

	if err := os.MkdirAll(archiveDir, 0o755); err != nil {
		return err
	}

	destPath := filepath.Join(archiveDir, name)
	if _, err := os.Stat(destPath); err == nil {
		stats.Exist++
		return nil
	} else if !os.IsNotExist(err) {
		return err
	}

	if err := os.Rename(path, destPath); err != nil {
		return err
	}
	stats.Moved++
	return nil
}

func extractTakenMonth(fileName string) string {
	base := fsutil.BaseWithoutExt(fileName)
	if len(base) < 8 {
		return ""
	}
	prefix8 := base[:8]
	if _, err := time.Parse("20060102", prefix8); err != nil {
		return ""
	}
	return prefix8[:6]
}
