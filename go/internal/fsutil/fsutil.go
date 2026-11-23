package fsutil

import (
	"os"
	"path/filepath"
	"strings"
)

// ExtLower returns the file extension without the leading dot, lowercased.
func ExtLower(name string) string {
	ext := filepath.Ext(name)
	if ext == "" {
		return ""
	}
	return strings.ToLower(strings.TrimPrefix(ext, "."))
}

// BaseWithoutExt returns the filename without its extension.
func BaseWithoutExt(name string) string {
	base := filepath.Base(name)
	ext := filepath.Ext(base)
	if ext == "" {
		return base
	}
	return base[:len(base)-len(ext)]
}

// ListFiles returns non-directory entries in dir whose extension is in accepted.
func ListFiles(dir string, accepted map[string]struct{}) ([]string, error) {
	entries, err := os.ReadDir(dir)
	if err != nil {
		return nil, err
	}
	var files []string
	for _, e := range entries {
		if e.IsDir() {
			continue
		}
		ext := ExtLower(e.Name())
		if _, ok := accepted[ext]; !ok {
			continue
		}
		files = append(files, filepath.Join(dir, e.Name()))
	}
	return files, nil
}

// ExtensionSet builds a set from a list of lowercase extensions.
func ExtensionSet(exts ...[]string) map[string]struct{} {
	set := make(map[string]struct{})
	for _, group := range exts {
		for _, e := range group {
			set[strings.ToLower(e)] = struct{}{}
		}
	}
	return set
}
