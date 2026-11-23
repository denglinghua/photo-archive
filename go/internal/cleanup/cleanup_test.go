package cleanup

import (
	"os"
	"path/filepath"
	"testing"
)

func TestRunDeletesMatchingPatterns(t *testing.T) {
	dir := t.TempDir()
	mustWrite(t, filepath.Join(dir, "IMG_0001.aae"), "sidecar")
	mustWrite(t, filepath.Join(dir, "keep.jpg"), "photo")
	mustWrite(t, filepath.Join(dir, "notes.txt"), "keep")

	stats, err := Run(dir, []string{"*.aae"})
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if stats.Deleted != 1 {
		t.Fatalf("Deleted=%d, want 1", stats.Deleted)
	}
	if stats.Error != 0 {
		t.Fatalf("Error=%d, want 0", stats.Error)
	}
	if _, err := os.Stat(filepath.Join(dir, "IMG_0001.aae")); !os.IsNotExist(err) {
		t.Fatalf(".aae should be deleted, err=%v", err)
	}
	if _, err := os.Stat(filepath.Join(dir, "keep.jpg")); err != nil {
		t.Fatalf("keep.jpg should remain: %v", err)
	}
	if _, err := os.Stat(filepath.Join(dir, "notes.txt")); err != nil {
		t.Fatalf("notes.txt should remain: %v", err)
	}
}

func TestRunCaseInsensitiveExtension(t *testing.T) {
	dir := t.TempDir()
	mustWrite(t, filepath.Join(dir, "edit.AAE"), "sidecar")

	stats, err := Run(dir, []string{"*.aae"})
	if err != nil {
		t.Fatalf("Run: %v", err)
	}
	if stats.Deleted != 1 {
		t.Fatalf("Deleted=%d, want 1", stats.Deleted)
	}
}

func mustWrite(t *testing.T, path, body string) {
	t.Helper()
	if err := os.WriteFile(path, []byte(body), 0o644); err != nil {
		t.Fatalf("write %s: %v", path, err)
	}
}
