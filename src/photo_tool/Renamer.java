package photo_tool;

import java.io.File;
import java.io.FileFilter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Date;
import java.util.HashSet;
import java.util.Set;
import java.util.TimeZone;

import com.drew.imaging.ImageMetadataReader;
import com.drew.metadata.Metadata;
import com.drew.metadata.exif.ExifSubIFDDirectory;
import com.drew.metadata.file.FileMetadataDirectory;

public class Renamer {

	public static void main(String[] args) {
		String photoDir;
		if (args.length < 1) {
			log("Usage:cmd photo_dir");
			return;
		} else {
			photoDir = args[0];
		}

		new Renamer(photoDir).rename();
	}

	String photoDir;
	Counter counter;

	public Renamer(final String photoDir) {
		this.photoDir = photoDir.trim();
	}

	void rename() {
		final File photoDirPath = new File(photoDir);
		if (!photoDirPath.exists()) {
			logError(String.format("Photo dir '%s' not exists.", this.photoDir));
			return;
		}

		log("start...");
		log("photo directory : " + photoDir);

		File[] photoList = getPhotoList(photoDirPath);

		counter = new Counter(photoList.length);

		for (File photo : photoList) {
			renamePhotoByDate(photo);
		}

		log(counter.toString());

		log("completed.");
	}

	void renamePhotoByDate(final File photo) {
		Date photoDate;
		try {
			photoDate = getPhotoCreationDate(photo);
		} catch (Exception e) {
			System.err.printf("extract date from photo '%s' error", photo.getName());
			counter.error++;
			return;
		}

		final DateFormat format = new SimpleDateFormat("yyyyMMdd_HHmmss");
		String newPhotoName = format.format(photoDate);

		File newPhotoFile = new File(photo.getParent(), newPhotoName + "." + getFileExtension(photo.getName()));
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

	private File[] getPhotoList(File photoDirPath) {
		final Set<String> acceptedExtensions = new HashSet<String>(Arrays.asList("jpg", "jpeg", "png", "mov"));

		// TODO process subdirectories recursively?
		final File[] photoList = photoDirPath.listFiles(new FileFilter() {
			public boolean accept(final File file) {
				if (file.isDirectory())
					return false;
				String extension = getFileExtension(file.getName()).toLowerCase();
				if (!acceptedExtensions.contains(extension))
					return false;
				return true;
			}
		});

		return photoList;
	}

	// no '.' in file path dir
	static String getFileExtension(String fileName) {
		String extension = "";

		int i = fileName.lastIndexOf('.');
		if (i > 0) {
			extension = fileName.substring(i + 1);
		}

		return extension;
	}

	static Date getPhotoCreationDate(File photo) throws Exception {
		Metadata metadata = ImageMetadataReader.readMetadata(photo);
		ExifSubIFDDirectory exifDir = metadata.getFirstDirectoryOfType(ExifSubIFDDirectory.class);
		Date creationDate;
		TimeZone timeZone = TimeZone.getDefault();
		if (exifDir != null) {
			creationDate = exifDir.getDate(ExifSubIFDDirectory.TAG_DATETIME_ORIGINAL, timeZone);
		} else {
			// if no EXIF, return last modified date
			FileMetadataDirectory fileDir = metadata.getFirstDirectoryOfType(FileMetadataDirectory.class);
			creationDate = fileDir.getDate(FileMetadataDirectory.TAG_FILE_MODIFIED_DATE, timeZone);
		}
		return creationDate;
	}

	static void log(String msg) {
		System.out.println(msg);
	}

	static void logError(String msg) {
		System.err.println(msg);
	}

	class Counter {
		public int total;
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
