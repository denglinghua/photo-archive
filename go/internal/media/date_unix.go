//go:build !windows

package media

import (
	"os"
	"syscall"
	"time"
)

func birthTime(fi os.FileInfo) (time.Time, bool) {
	stat, ok := fi.Sys().(*syscall.Stat_t)
	if !ok {
		return time.Time{}, false
	}
	// Birth time is available on Darwin / BSD via Birthtimespec.
	sec, nsec := birthTimespec(stat)
	if sec == 0 && nsec == 0 {
		return time.Time{}, false
	}
	return time.Unix(sec, nsec), true
}
