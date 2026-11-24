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
	cfgPath, err := config.PathBesideExecutable()
	if err != nil {
		fmt.Fprintln(os.Stderr, "locate config:", err)
		os.Exit(1)
	}

	cfg, err := config.Load(cfgPath)
	if err != nil {
		fmt.Fprintf(os.Stderr, "config error: %v\n\nPlace %s next to the executable, for example:\n%s\n",
			err, config.FileName, config.ExampleJSON)
		os.Exit(1)
	}

	if err := requireDir(cfg.SourceDir, "source dir"); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}
	if err := requireDir(cfg.DestDir, "dest dir"); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}
	if err := requireDir(cfg.VideoDestDir, "video dest dir"); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}

	fmt.Println("start...")
	fmt.Println("config :", cfgPath)
	fmt.Println("source directory :", cfg.SourceDir)
	fmt.Println("dest directory :", cfg.DestDir)
	fmt.Println("video dest directory :", cfg.VideoDestDir)

	fmt.Println("\n-- rename --")
	renameStats, err := rename.Run(cfg.SourceDir, cfg)
	if err != nil {
		fmt.Fprintln(os.Stderr, "rename failed:", err)
		os.Exit(1)
	}
	fmt.Println(renameStats.String())

	fmt.Println("\n-- cleanup --")
	cleanupStats, err := cleanup.Run(cfg.SourceDir, cfg.DeletePatterns)
	if err != nil {
		fmt.Fprintln(os.Stderr, "cleanup failed:", err)
		os.Exit(1)
	}
	fmt.Println(cleanupStats.String())

	fmt.Println("\n-- archive --")
	archiveStats, err := archive.Run(cfg)
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
