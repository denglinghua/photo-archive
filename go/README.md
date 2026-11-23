# photo-archive (Go)

Rename photos/videos by taken date, then archive them into year/month folders.
Builds to a **single native binary** — no Java or other runtime on the machine that runs it.

## Usage

```bash
photo-archive <source_dir> <dest_dir>
```

1. **Rename** files in `source_dir` to `yyyyMMdd_HHmmss.ext` (EXIF `DateTimeOriginal` for images; file creation/mod time for videos).
2. **Cleanup** delete top-level files matching `DeletePatterns` in `internal/config/config.go` (default: `*.AAE`).
3. **Archive** into `dest_dir`:
   - photos → `dest/yyyy/yyyyMM/`
   - videos → `dest/yyyy/`

Supported extensions: `jpg`, `jpeg`, `png`, `heic`, `mov`, `avi`, `mp4`, `m4v` (top-level only, not recursive).

## Build

Requires [Go](https://go.dev/dl/) on the **build** machine only.

```bash
cd go
go build -o photo-archive ./cmd/photo-archive
```

### Cross-compile

```bash
# Mac Apple Silicon
GOOS=darwin GOARCH=arm64 go build -o photo-archive-darwin-arm64 ./cmd/photo-archive

# Mac Intel
GOOS=darwin GOARCH=amd64 go build -o photo-archive-darwin-amd64 ./cmd/photo-archive

# Windows
GOOS=windows GOARCH=amd64 go build -o photo-archive.exe ./cmd/photo-archive
```

Copy the binary to the target machine and run it — no Go install needed there.
