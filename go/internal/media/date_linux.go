//go:build linux

package media

import "syscall"

func birthTimespec(stat *syscall.Stat_t) (sec int64, nsec int64) {
	// Linux Stat_t has no reliable birth time in portable Go; use zero.
	return 0, 0
}
