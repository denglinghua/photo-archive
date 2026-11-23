//go:build darwin

package media

import "syscall"

func birthTimespec(stat *syscall.Stat_t) (sec int64, nsec int64) {
	return stat.Birthtimespec.Sec, int64(stat.Birthtimespec.Nsec)
}
