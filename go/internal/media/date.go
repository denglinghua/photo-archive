package media

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"time"

	"github.com/bep/imagemeta"

	"photo-archive/internal/config"
	"photo-archive/internal/fsutil"
)

var (
	imageExts = fsutil.ExtensionSet(config.ImageExtensions)
	videoExts = fsutil.ExtensionSet(config.VideoExtensions)
)

// IsImage reports whether ext (lowercase, no dot) is a supported image type.
func IsImage(ext string) bool {
	_, ok := imageExts[ext]
	return ok
}

// IsVideo reports whether ext (lowercase, no dot) is a supported video type.
func IsVideo(ext string) bool {
	_, ok := videoExts[ext]
	return ok
}

// TakenDate returns the media capture/creation time for path.
// Photos: EXIF DateTimeOriginal, else file mod time.
// Videos: min(birthTime, modTime).
func TakenDate(path string) (time.Time, error) {
	ext := fsutil.ExtLower(path)
	if IsVideo(ext) {
		return videoCreationDate(path)
	}
	if IsImage(ext) {
		return photoTakenDate(path)
	}
	return time.Time{}, fmt.Errorf("unsupported extension: %s", ext)
}

func photoTakenDate(path string) (time.Time, error) {
	f, err := os.Open(path)
	if err != nil {
		return time.Time{}, err
	}
	defer f.Close()

	format, err := imageFormatForExt(fsutil.ExtLower(path))
	if err != nil {
		return time.Time{}, err
	}

	var tags imagemeta.Tags
	_, err = imagemeta.Decode(imagemeta.Options{
		R:           f,
		ImageFormat: format,
		Sources:     imagemeta.EXIF,
		HandleTag: func(ti imagemeta.TagInfo) error {
			tags.Add(ti)
			return nil
		},
		ShouldHandleTag: func(ti imagemeta.TagInfo) bool {
			if ti.Source != imagemeta.EXIF {
				return false
			}
			// Keep IFD0 (+ ExifIFD) so DateTimeOriginal is reachable.
			return strings.HasPrefix(ti.Namespace, "IFD0")
		},
	})
	if err != nil {
		return time.Time{}, err
	}

	dt, err := tags.GetDateTime()
	if err != nil {
		return time.Time{}, err
	}
	if !dt.IsZero() {
		return dt, nil
	}

	fi, err := os.Stat(path)
	if err != nil {
		return time.Time{}, err
	}
	fmt.Printf("no taken date : %s\n", filepath.Base(path))
	return fi.ModTime(), nil
}

func videoCreationDate(path string) (time.Time, error) {
	fi, err := os.Stat(path)
	if err != nil {
		return time.Time{}, err
	}
	mod := fi.ModTime()
	if created, ok := birthTime(fi); ok {
		if created.Before(mod) {
			return created, nil
		}
	}
	return mod, nil
}

func imageFormatForExt(ext string) (imagemeta.ImageFormat, error) {
	switch ext {
	case "jpg", "jpeg":
		return imagemeta.JPEG, nil
	case "png":
		return imagemeta.PNG, nil
	case "heic":
		return imagemeta.HEIF, nil
	default:
		return 0, fmt.Errorf("no image format for extension: %s", ext)
	}
}
