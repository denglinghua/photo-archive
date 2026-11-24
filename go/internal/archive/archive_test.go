package archive

import (
	"os"
	"path/filepath"
	"testing"

	"photo-archive/internal/config"
)

func TestRunRoutesPhotosAndVideosToSeparateRoots(t *testing.T) {
	src := t.TempDir()
	photoDest := t.TempDir()
	videoDest := t.TempDir()

	mustWrite(t, filepath.Join(src, "20260115_120000.jpg"), "photo")
	mustWrite(t, filepath.Join(src, "20260115_130000.mp4"), "video")

	cfg := config.Config{
		SourceDir:       src,
		DestDir:         photoDest,
		VideoDestDir:    videoDest,
		ImageExtensions: []string{"jpg"},
		VideoExtensions: []string{"mp4"},
	}

	stats, err := Run(cfg)
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if stats.Moved != 2 {
		t.Fatalf("Moved=%d, want 2", stats.Moved)
	}

	photoPath := filepath.Join(photoDest, "2026", "202601", "20260115_120000.jpg")
	videoPath := filepath.Join(videoDest, "2026", "20260115_130000.mp4")
	if _, err := os.Stat(photoPath); err != nil {
		t.Fatalf("photo dest missing: %v", err)
	}
	if _, err := os.Stat(videoPath); err != nil {
		t.Fatalf("video dest missing: %v", err)
	}
}

func mustWrite(t *testing.T, path, body string) {
	t.Helper()
	if err := os.WriteFile(path, []byte(body), 0o644); err != nil {
		t.Fatal(err)
	}
}
