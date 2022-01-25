package photo_archive;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.nio.file.attribute.BasicFileAttributes;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Collections;
import java.util.Date;
import java.util.HashSet;
import java.util.Set;
import java.util.TimeZone;

import com.drew.imaging.ImageMetadataReader;
import com.drew.metadata.Directory;
import com.drew.metadata.Metadata;
import com.drew.metadata.Tag;
import com.drew.metadata.exif.ExifSubIFDDirectory;
import com.drew.metadata.file.FileSystemDirectory;

public class Renamer {

	public static void main(String[] args) {
		final String photoDir;
		if (args.length < 1) {
			Utils.log("usage:cmd photo_dir");
			return;
		} else {
			photoDir = args[0];
		}

		new Renamer(photoDir).rename();
	}

	private final static Set<String> imageExtensions = new HashSet<String>(Config.imageExtentions);
	private final static Set<String> videoExtensions = new HashSet<String>(Config.videoExtensions);
	private final String photoDir;
	private Counter counter;

	public Renamer(String photoDir) {
		this.photoDir = photoDir.trim();
	}

	void rename() {
		final File photoDirPath = new File(photoDir);
		if (!photoDirPath.exists()) {
			Utils.logError(String.format("photo dir '%s' not exists.", this.photoDir));
			return;
		}

		Utils.log("start...");
		Utils.log("photo directory : " + photoDir);

		final File[] photoList = getPhotoList(photoDirPath);
		// IMG_####, IMG_E#### the latter image is which a filter applied to
		// those 2 images have the same taken time
		// To keep the latter, order the array to make it in the front of the former
		// which will be duplicated
		Arrays.sort(photoList, Collections.reverseOrder());

		counter = new Counter(photoList.length);

		for (File photo : photoList) {
			renamePhotoByDate(photo);
		}

		Utils.log(counter.toString());

		Utils.log("completed.");
	}

	private void renamePhotoByDate(final File photo) {
		final Date photoDate;
		final String fileExtension = Utils.getFileExtension(photo.getName()).toLowerCase();
		try {
			boolean isVideo = videoExtensions.contains(fileExtension);
			photoDate = isVideo ? getVideoCreationDate(photo) : getPhotoTakenDate(photo);
		} catch (Exception e) {
			System.err.printf("extract date from photo '%s' error", photo.getName());
			counter.error++;
			return;
		}

		final DateFormat format = new SimpleDateFormat("yyyyMMdd_HHmmss");
		String newPhotoName = format.format(photoDate);

		File newPhotoFile = new File(photo.getParent(), newPhotoName + "." + fileExtension);
		if (!newPhotoFile.exists()) {
			try {
				Files.move(photo.toPath(), newPhotoFile.toPath(), StandardCopyOption.REPLACE_EXISTING);
				counter.changed++;
			} catch (IOException e) {
				e.printStackTrace();
			}
		} else {
			counter.duplicated++;
		}
	}

	private File[] getPhotoList(final File photoDirPath) {
		Set<String> acceptedExtensions = new HashSet<String>();
		acceptedExtensions.addAll(imageExtensions);
		acceptedExtensions.addAll(videoExtensions);

		return Utils.getFileList(photoDirPath, acceptedExtensions);
	}

	private Date getPhotoTakenDate(File file) throws Exception {
		Metadata metadata = ImageMetadataReader.readMetadata(file);
		ExifSubIFDDirectory exifDir = metadata.getFirstDirectoryOfType(ExifSubIFDDirectory.class);
		Date takenDate = null;
		TimeZone timeZone = TimeZone.getDefault();
		if (exifDir != null) {
			takenDate = exifDir.getDate(ExifSubIFDDirectory.TAG_DATETIME_ORIGINAL, timeZone);
		}

		if (exifDir == null || takenDate == null) {
			// if no EXIF, return last modified date
			FileSystemDirectory fileDir = metadata.getFirstDirectoryOfType(FileSystemDirectory.class);
			takenDate = fileDir.getDate(FileSystemDirectory.TAG_FILE_MODIFIED_DATE, timeZone);
		}

		return takenDate;
	}

	private Date getVideoCreationDate(File file) throws IOException {
		BasicFileAttributes attr = Files.readAttributes(file.toPath(), BasicFileAttributes.class);
		return new Date(Math.min(attr.creationTime().toMillis(), attr.lastModifiedTime().toMillis()));
	}

	@SuppressWarnings("unused")
	private static void printMetadata(Metadata metadata) {
		System.out.println("-------------------------------------");

		for (Directory directory : metadata.getDirectories()) {

			System.out.println(directory.getClass());

			for (Tag tag : directory.getTags()) {
				System.out.println(tag);
			}

			if (directory.hasErrors()) {
				for (String error : directory.getErrors()) {
					System.err.println("ERROR: " + error);
				}
			}
		}
	}

	private class Counter {
		private final int total;
		public int changed;
		public int duplicated;
		public int error;

		public Counter(int total) {
			this.total = total;
		}

		@Override
		public String toString() {
			return " total=" + total + "\n changed=" + changed + "\n duplicated=" + duplicated + "\n error=" + error;
		}
	}
}
