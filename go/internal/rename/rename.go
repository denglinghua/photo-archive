package rename

import (
	"fmt"
	"os"
	"path/filepath"
	"sort"

	"photo-archive/internal/config"
	"photo-archive/internal/fsutil"
	"photo-archive/internal/media"
)

// Stats holds rename counters.
type Stats struct {
	Total      int
	Changed    int
	Duplicated int
	Error      int
}

func (s Stats) String() string {
	return fmt.Sprintf(" total=%d\n changed=%d\n duplicated=%d\n error=%d",
		s.Total, s.Changed, s.Duplicated, s.Error)
}

// Run renames media files in dir by taken/creation date to yyyyMMdd_HHmmss.ext.
func Run(dir string, cfg config.Config) (Stats, error) {
	files, err := fsutil.ListFiles(dir, cfg.AcceptedExtensions())
	if err != nil {
		return Stats{}, err
	}

	// Reverse sort so IMG_E#### is processed before IMG_#### when times collide.
	sort.Slice(files, func(i, j int) bool {
		return filepath.Base(files[i]) > filepath.Base(files[j])
	})

	stats := Stats{Total: len(files)}
	for _, path := range files {
		if err := renameOne(path, cfg, &stats); err != nil {
			fmt.Fprintf(os.Stderr, "extract date from photo '%s' error: %v\n", filepath.Base(path), err)
			stats.Error++
		}
	}
	return stats, nil
}

func renameOne(path string, cfg config.Config, stats *Stats) error {
	taken, err := media.TakenDate(path, cfg)
	if err != nil {
		return err
	}

	ext := fsutil.ExtLower(path)
	newName := taken.Format("20060102_150405") + "." + ext
	newPath := filepath.Join(filepath.Dir(path), newName)

	if path == newPath {
		stats.Duplicated++
		return nil
	}
	if _, err := os.Stat(newPath); err == nil {
		stats.Duplicated++
		return nil
	} else if !os.IsNotExist(err) {
		return err
	}

	if err := os.Rename(path, newPath); err != nil {
		return err
	}
	stats.Changed++
	return nil
}
