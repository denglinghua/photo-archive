package cleanup

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"
)

// Stats holds cleanup counters.
type Stats struct {
	Deleted int
	Error   int
}

func (s Stats) String() string {
	return fmt.Sprintf(" deleted=%d\n error=%d", s.Deleted, s.Error)
}

// Run deletes top-level files in dir that match any of the glob patterns
// (e.g. "*.aae"). Matching is case-insensitive on the filename.
func Run(dir string, patterns []string) (Stats, error) {
	var stats Stats
	if len(patterns) == 0 {
		return stats, nil
	}

	entries, err := os.ReadDir(dir)
	if err != nil {
		return stats, err
	}

	for _, e := range entries {
		if e.IsDir() {
			continue
		}
		name := e.Name()
		if !matchesAny(name, patterns) {
			continue
		}
		path := filepath.Join(dir, name)
		if err := os.Remove(path); err != nil {
			fmt.Fprintf(os.Stderr, "delete '%s' error: %v\n", name, err)
			stats.Error++
			continue
		}
		stats.Deleted++
	}
	return stats, nil
}

func matchesAny(name string, patterns []string) bool {
	lowerName := strings.ToLower(name)
	for _, pattern := range patterns {
		matched, err := filepath.Match(strings.ToLower(pattern), lowerName)
		if err != nil {
			continue
		}
		if matched {
			return true
		}
	}
	return false
}
