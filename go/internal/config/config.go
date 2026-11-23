package config

// ImageExtensions are supported still-image file extensions (lowercase, no dot).
var ImageExtensions = []string{"jpg", "jpeg", "png", "heic"}

// VideoExtensions are supported video file extensions (lowercase, no dot).
var VideoExtensions = []string{"mov", "avi", "mp4", "m4v"}

// DeletePatterns are globs matched against top-level source filenames after rename.
// Matching files are deleted before archive (e.g. Apple Photos sidecars).
var DeletePatterns = []string{"*.AAE"}
