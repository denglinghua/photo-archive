package photo_archive;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

public class Archiver {
	public static void main(String[] args) {
		if (args.length < 3) {
			Utils.log("usage:cmd source_dir dest_dir [photo/video]");
			return;
		}

		new Archiver(args[0], args[1], args[2]).archive();
	}

	private Set<String> fileExtensions;
	private final String sourceDir;
	private final String destDir;
	private final String type;
	private final Map<String, File> archiveDirs = new HashMap<String, File>();
	private Counter counter;

	public Archiver(String sourceDir, String destDir, String type) {
		this.sourceDir = sourceDir.trim();
		this.destDir = destDir.trim();
		this.type = type.trim();

		this.fileExtensions = new HashSet<String>(type.equals("photo") ? Config.imageExtentions : Config.videoExtensions);
	}

	public void archive() {
		final File sourceDirPath = new File(this.sourceDir);
		final File destDirPath = new File(this.destDir);
		if (!sourceDirPath.exists()) {
			Utils.logError(String.format("source dir '%s' not exists.", this.sourceDir));
			return;
		}
		if (!destDirPath.exists()) {
			Utils.logError(String.format("dest dir '%s' not exists.", this.destDir));
			return;
		}

		Utils.log("start...");
		Utils.log("source directory : " + this.sourceDir);
		Utils.log("dest directory : " + this.destDir);

		this.archiveDirs.clear();

		final File[] fileList = Utils.getFileList(sourceDirPath, fileExtensions);
		this.counter = new Counter(fileList.length);

		for (File file : fileList) {
			moveToArchiveDir(file);
		}

		Utils.log(counter.toString());

		Utils.log("completed.");
	}

	private void moveToArchiveDir(File file) {
		final String takenMonth = extractPhotoTakenMonth(file.getName());
		if (takenMonth == null) {
			this.counter.ignored++;
			return;
		}
		final String takenYear = takenMonth.substring(0, 4);
		// photo archived by yyyyMM, video by yyyy
		final String dirKey = this.type.equals("photo") ? takenMonth : takenYear;

		File archiveDir;
		if (archiveDirs.containsKey(dirKey)) {
			archiveDir = archiveDirs.get(dirKey);
		} else {
			File yearDir = new File(this.destDir, takenYear);
			yearDir.mkdir();
			if (this.type.equals("photo")) {
				archiveDir = new File(yearDir, takenMonth);
				archiveDir.mkdir();
			} else {
				archiveDir = yearDir;
			}

			archiveDirs.put(dirKey, archiveDir);
		}

		File destFile = new File(archiveDir, file.getName());
		if (destFile.exists()) {
			this.counter.exist++;
			return;
		}

		try {
			Files.move(file.toPath(), destFile.toPath(), StandardCopyOption.REPLACE_EXISTING);
			this.counter.moved++;
		} catch (IOException e) {
			e.printStackTrace();
			this.counter.error++;
		}
	}

	private static String extractPhotoTakenMonth(String fileName) {
		String nameWithoutExt = Utils.getNameWithoutExtension(fileName);
		if (nameWithoutExt.length() < 8)
			return null;
		String prefix8 = nameWithoutExt.substring(0, 8);
		final DateFormat format = new SimpleDateFormat("yyyyMMdd");
		try {
			format.parse(prefix8);
			return prefix8.substring(0, 6);
		} catch (ParseException e) {
			return null;
		}
	}

	private class Counter {
		private final int total;
		public int ignored;
		public int exist;
		public int moved;
		public int error;

		public Counter(int total) {
			this.total = total;
		}

		@Override
		public String toString() {
			return " total=" + total + "\n moved=" + moved + "\n exist=" + exist + "\n ignored=" + ignored
					+ " \n error=" + error;
		}
	}
}
