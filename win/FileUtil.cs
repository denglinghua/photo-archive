using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace PhotoRenamer
{
    static class FileUtil
    {
        public static String GetPhotoTakenMonth(String fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            //The name of the file includes the file extension.
            DateTime? takenDate = ExtractFileNameDate(fileInfo.FullName);
            if (takenDate != null)
            {
               return String.Format("{0:yyyyMM}", takenDate);
            }
            else
            {
                return null;
            }
        }

        private static DateTime? ExtractFileNameDate(String fileName)
        {
            String nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            if (nameWithoutExt.Length < 8) return null;
            String prefix8 = nameWithoutExt.Substring(0, 8);
            DateTime date;
            bool ok = DateTime.TryParseExact(prefix8, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (ok)
                return date;
            else
                return null;
        }
    }
}
