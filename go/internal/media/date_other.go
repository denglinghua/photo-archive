//go:build !windows && !darwin && !linux

package media

import "syscall"

func birthTimespec(stat *syscall.Stat_t) (sec int64, nsec int64) {
	return 0, 0
}
