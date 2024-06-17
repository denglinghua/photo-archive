using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using PhotoRenamer.Properties;

namespace PhotoRenamer
{
    public partial class ArchiveForm : Form
    {
        string archiveDir;
        Dictionary<string, string> createdMonthDir = new Dictionary<string, string>();
        int ignoreCount = 0;
        int existCount = 0;
        int moveCount = 0;

        public ArchiveForm(string sourceDir)
        {
            InitializeComponent();

            this.textBoxSourceDir.Text = sourceDir;

            this.InitArchiveDir();
        }

        private void InitArchiveDir()
        {
            if (!string.IsNullOrEmpty(Settings.Default.ArchiveDir))
            {
                this.textBoxArchiveDir.Text = Settings.Default.InitDir;
            }
        }

        private void SaveArchiveDir()
        {
            Settings.Default.ArchiveDir = this.textBoxArchiveDir.Text.Trim();
            Settings.Default.Save();
        }


        private void buttonArchive_Click(object sender, EventArgs e)
        {
            if (!CheckDirTextBox(this.textBoxSourceDir, "Source"))
            {
                return;
            }

            if (!CheckDirTextBox(this.textBoxArchiveDir, "Archive"))
            {
                return;
            }

            string sourceDir = this.textBoxSourceDir.Text.Trim();
            this.archiveDir = this.textBoxArchiveDir.Text.Trim();
            this.createdMonthDir.Clear();
            this.ignoreCount = this.existCount = this.moveCount = 0;

            String[] folders;
            if (checkBoxSubFolders.Checked)
            {
                folders = Directory.GetDirectories(sourceDir);
            }
            else
            {
                folders = new string[] { sourceDir };
            }

            foreach (String folder in folders)
            {
                this.MoveFolderFiles(folder);
            }

            MessageBox.Show(String.Format("ignored : {0}, exist : {1}, moved : {2}", this.ignoreCount, this.existCount, this.moveCount));

            this.SaveArchiveDir();
        }

        private bool CheckDirTextBox(TextBox textBox, string dirName)
        {
            string dir = textBox.Text.Trim();

            if (String.IsNullOrEmpty(dir))
            {
                MessageBox.Show(string.Format("Please select {0} folder.", dirName),
                    string.Format("Select {0} folder.", dirName), MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox.Focus();
                return false;
            }

            if (!Directory.Exists(dir))
            {
                MessageBox.Show(
                    String.Format("The {0} folder is not exist.", dirName),
                    String.Format("{0} folder error", dirName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private void MoveFolderFiles(string sourceDir)
        {
            String[] sourceFiles = Directory.GetFiles(sourceDir, "*.JPG");

            foreach (string file in sourceFiles)
            {
                this.MoveFile(file);
            }
        }

        private void MoveFile(String file)
        {
            String takenMonth = FileUtil.GetPhotoTakenMonth(file);
            if (takenMonth == null)
            {
                this.ignoreCount++;
                return;
            }

            String monthDir;
            if (createdMonthDir.ContainsKey(takenMonth))
            {
                monthDir = createdMonthDir[takenMonth];
            }
            else
            {
                String yearDir = Path.Combine(this.archiveDir, takenMonth.Substring(0, 4));
                DirectoryInfo newDir = Directory.CreateDirectory(Path.Combine(yearDir, takenMonth));
                monthDir = newDir.FullName;
                createdMonthDir[takenMonth] = monthDir;
            }

            String destFile = Path.Combine(monthDir, Path.GetFileName(file));
            if (File.Exists(destFile))
            {
                this.existCount++;
                return;
            }

            File.Move(file, destFile);
            this.moveCount++;
        }
    }
}
