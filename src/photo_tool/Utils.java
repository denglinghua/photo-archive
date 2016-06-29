package photo_tool;

import java.io.File;
import java.io.FileFilter;
import java.util.Set;

import org.apache.commons.io.FilenameUtils;

public class Utils {
	// the name - file.txt
	// the base name - file
	// the extension - txt
	static String getName(String path) {
		return FilenameUtils.getName(path);
	}

	static String getNameWithoutExtension(String path) {
		return FilenameUtils.getBaseName(path);
	}

	static String getFileExtension(String path) {
		return FilenameUtils.getExtension(path);
	}

	static File[] getFileList(final File FileDir, Set<String> acceptedExtensions) {
		// TODO process subdirectories recursively?
		final File[] fileList = FileDir.listFiles(new FileFilter() {
			public boolean accept(final File file) {
				if (file.isDirectory())
					return false;
				String extension = Utils.getFileExtension(file.getName()).toLowerCase();
				if (!acceptedExtensions.contains(extension))
					return false;
				return true;
			}
		});

		return fileList;
	}

	static void log(String msg) {
		System.out.println(msg);
	}

	static void logError(String msg) {
		System.err.println(msg);
	}
}
