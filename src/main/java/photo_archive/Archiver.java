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
		if (args.length < 2) {
			Utils.log("usage:cmd source_dir dest_dir");
			return;
		}

		new Archiver(args[0], args[1]).archive();
	}

	private final static Set<String> photoExtensions = new HashSet<String>(Config.imageExtentions);
	private final String sourceDir;
	private final String destDir;
	private final Map<String, File> monthDirs = new HashMap<String, File>();
	private Counter counter;

	public Archiver(String sourceDir, String destDir) {
		this.sourceDir = sourceDir.trim();
		this.destDir = destDir.trim();
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

		this.monthDirs.clear();

		final File[] photoList = Utils.getFileList(sourceDirPath, photoExtensions);
		this.counter = new Counter(photoList.length);

		for (File photo : photoList) {
			moveToMonthDir(photo);
		}

		Utils.log(counter.toString());

		Utils.log("completed.");
	}

	private void moveToMonthDir(File photo) {
		String takenMonth = extractPhotoTakenMonth(photo.getName());
		if (takenMonth == null) {
			this.counter.ignored++;
			return;
		}

		File monthDir;
		if (monthDirs.containsKey(takenMonth)) {
			monthDir = monthDirs.get(takenMonth);
		} else {
			File yearDir = new File(this.destDir, takenMonth.substring(0, 4));
			yearDir.mkdir();
			monthDir = new File(yearDir, takenMonth);
			monthDir.mkdir();
			monthDirs.put(takenMonth, monthDir);
		}

		File destFile = new File(monthDir, photo.getName());
		if (destFile.exists()) {
			this.counter.exist++;
			return;
		}

		try {
			Files.move(photo.toPath(), destFile.toPath(), StandardCopyOption.REPLACE_EXISTING);
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
