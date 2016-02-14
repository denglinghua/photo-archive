using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace PhotoRenamer
{
    class UpdateTimeRenamer : Renamer
    {
        public UpdateTimeRenamer(string dir, Log log, bool recursive)
            : base(dir, log, recursive)
        {
        }

        public override void Rename()
        {
            int ignored = 0;
            int changed = 0;
            String[] files = Directory.GetFiles(this.dir, "*.*");
            foreach (String file in files)
            {
                if (isFileNameDate(file))
                {
                    ignored++;
                    continue;
                }

                DateTime createdTime = File.GetCreationTime(file);
                DateTime lastUpdatedTime = File.GetLastWriteTime(file);

                DateTime fileNameDate = createdTime > lastUpdatedTime ? lastUpdatedTime : createdTime;

                String newFileNameWithoutExt = String.Format("{0:yyyyMMdd_HHmmss}", fileNameDate);
                String newFileExt = Path.GetExtension(file);
                String newFileName = newFileNameWithoutExt + newFileExt;
                String newFileFullName = Path.Combine(Path.GetDirectoryName(file), newFileName);

                if (!File.Exists(newFileFullName))
                {
                    File.Move(file, newFileFullName);
                    changed++;
                }
                else
                {
                    ignored++;
                }
            }

            this.log("Finished.");
            this.log(string.Format("{0} files changed.", changed));
            this.log(string.Format("{0} files ignored.", ignored));
        }

        private static bool isFileNameDate(String fileName)
        {
            String nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            if (nameWithoutExt.Length < 8) return false;
            String prefix8 = nameWithoutExt.Substring(0, 8);
            DateTime date;
            return DateTime.TryParseExact(prefix8, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
