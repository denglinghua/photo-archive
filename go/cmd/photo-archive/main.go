package main

import (
	"fmt"
	"os"

	"photo-archive/internal/archive"
	"photo-archive/internal/cleanup"
	"photo-archive/internal/config"
	"photo-archive/internal/rename"
)

func main() {
	if len(os.Args) < 3 {
		fmt.Fprintln(os.Stderr, "usage: photo-archive <source_dir> <dest_dir>")
		os.Exit(1)
	}

	sourceDir := os.Args[1]
	destDir := os.Args[2]

	if err := requireDir(sourceDir, "source dir"); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}
	if err := requireDir(destDir, "dest dir"); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}

	fmt.Println("start...")
	fmt.Println("source directory :", sourceDir)
	fmt.Println("dest directory :", destDir)

	fmt.Println("\n-- rename --")
	renameStats, err := rename.Run(sourceDir)
	if err != nil {
		fmt.Fprintln(os.Stderr, "rename failed:", err)
		os.Exit(1)
	}
	fmt.Println(renameStats.String())

	fmt.Println("\n-- cleanup --")
	cleanupStats, err := cleanup.Run(sourceDir, config.DeletePatterns)
	if err != nil {
		fmt.Fprintln(os.Stderr, "cleanup failed:", err)
		os.Exit(1)
	}
	fmt.Println(cleanupStats.String())

	fmt.Println("\n-- archive --")
	archiveStats, err := archive.Run(sourceDir, destDir)
	if err != nil {
		fmt.Fprintln(os.Stderr, "archive failed:", err)
		os.Exit(1)
	}
	fmt.Println(archiveStats.String())

	fmt.Println("\ncompleted.")
}

func requireDir(path, label string) error {
	fi, err := os.Stat(path)
	if err != nil {
		if os.IsNotExist(err) {
			return fmt.Errorf("%s '%s' not exists.", label, path)
		}
		return err
	}
	if !fi.IsDir() {
		return fmt.Errorf("%s '%s' is not a directory.", label, path)
	}
	return nil
}
