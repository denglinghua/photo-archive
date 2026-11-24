# photo-archive (Go)

Rename photos/videos by taken date, then archive them into year/month folders.
Builds to a **single native binary** — no Java or other runtime on the machine that runs it.

## Usage

1. Put `photo-archive.json` **next to the executable** (same folder).
2. Edit paths and options, then run:

```bash
photo-archive
```

Example config:

```json
{
  "source_dir": "/path/to/inbox",
  "dest_dir": "/path/to/photo-archive",
  "video_dest_dir": "/path/to/video-archive",
  "image_extensions": ["jpg", "jpeg", "png", "heic"],
  "video_extensions": ["mov", "avi", "mp4", "m4v"],
  "delete_patterns": ["*.AAE"]
}
```

Pipeline:

1. **Rename** files in `source_dir` to `yyyyMMdd_HHmmss.ext` (EXIF `DateTimeOriginal` for images; file creation/mod time for videos).
2. **Cleanup** delete top-level files matching `delete_patterns`.
3. **Archive**:
   - photos → `dest_dir/yyyy/yyyyMM/`
   - videos → `video_dest_dir/yyyy/`

`source_dir`, `dest_dir`, and `video_dest_dir` must already exist as directories. Extensions and paths are read from the JSON at runtime (change anytime without rebuilding). Top-level only, not recursive.

## Build

Requires [Go](https://go.dev/dl/) on the **build** machine only.

```bash
cd go
go build -o photo-archive ./cmd/photo-archive
# copy photo-archive.json next to the binary and edit paths
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

Copy the binary **and** `photo-archive.json` to the target machine — no Go install needed there.
