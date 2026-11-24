package config

import (
	"os"
	"path/filepath"
	"testing"
)

func TestLoadValidJSON(t *testing.T) {
	dir := t.TempDir()
	path := filepath.Join(dir, "photo-archive.json")
	body := `{
  "source_dir": "C:/inbox",
  "dest_dir": "C:/photos",
  "video_dest_dir": "C:/videos",
  "image_extensions": ["jpg", "HEIC"],
  "video_extensions": ["mov", "MP4"],
  "delete_patterns": ["*.AAE"]
}`
	if err := os.WriteFile(path, []byte(body), 0o644); err != nil {
		t.Fatal(err)
	}

	cfg, err := Load(path)
	if err != nil {
		t.Fatalf("Load: %v", err)
	}
	if cfg.SourceDir != "C:/inbox" || cfg.DestDir != "C:/photos" || cfg.VideoDestDir != "C:/videos" {
		t.Fatalf("paths: %+v", cfg)
	}
	if len(cfg.ImageExtensions) != 2 || cfg.ImageExtensions[0] != "jpg" || cfg.ImageExtensions[1] != "heic" {
		t.Fatalf("image_extensions normalized: %v", cfg.ImageExtensions)
	}
	if len(cfg.VideoExtensions) != 2 || cfg.VideoExtensions[1] != "mp4" {
		t.Fatalf("video_extensions normalized: %v", cfg.VideoExtensions)
	}
	if len(cfg.DeletePatterns) != 1 || cfg.DeletePatterns[0] != "*.AAE" {
		t.Fatalf("delete_patterns: %v", cfg.DeletePatterns)
	}
}

func TestLoadMissingVideoDestDir(t *testing.T) {
	dir := t.TempDir()
	path := filepath.Join(dir, "photo-archive.json")
	body := `{"source_dir":"a","dest_dir":"b","video_dest_dir":"","image_extensions":["jpg"],"video_extensions":["mov"]}`
	if err := os.WriteFile(path, []byte(body), 0o644); err != nil {
		t.Fatal(err)
	}
	if _, err := Load(path); err == nil {
		t.Fatal("expected error for empty video_dest_dir")
	}
}

func TestLoadMissingSourceOrDest(t *testing.T) {
	dir := t.TempDir()
	path := filepath.Join(dir, "photo-archive.json")
	if err := os.WriteFile(path, []byte(`{"source_dir":"","dest_dir":"x","video_dest_dir":"y","image_extensions":["jpg"],"video_extensions":["mov"]}`), 0o644); err != nil {
		t.Fatal(err)
	}
	if _, err := Load(path); err == nil {
		t.Fatal("expected error for empty source_dir")
	}
}

func TestLoadMissingFile(t *testing.T) {
	_, err := Load(filepath.Join(t.TempDir(), "missing.json"))
	if err == nil {
		t.Fatal("expected error")
	}
}

func TestPathBesideExecutable(t *testing.T) {
	p, err := PathBesideExecutable()
	if err != nil {
		t.Fatal(err)
	}
	if filepath.Base(p) != "photo-archive.json" {
		t.Fatalf("base=%s, want photo-archive.json", filepath.Base(p))
	}
}
