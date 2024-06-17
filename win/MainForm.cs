using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using PhotoRenamer.Properties;


namespace PhotoRenamer
{
    public partial class MainForm : Form
    {
        private const string VerNo = "20160214";
        private readonly string appDir;
        private string photoDir;

        public MainForm()
        {
            InitializeComponent();

            this.appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            this.CheckExifToolDependency();

            this.ShowVersionNo();

            this.InitSelectDir();
        }

        private void CheckExifToolDependency()
        {
            String exifToolFile = ExifToolRenamer.ExifToolFile;
            if (!File.Exists(Path.Combine(this.appDir, exifToolFile)))
            {
                MessageBox.Show(string.Format("Exif tool file '{0}' is not found, Application exit.", exifToolFile), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                //Application.Exit();
            }
        }

        private void ShowVersionNo()
        {
            this.Text = string.Format(this.Text, VerNo);
        }

        private void InitSelectDir()
        {
            if (!string.IsNullOrEmpty(Settings.Default.InitDir))
            {
                this.folderBrowserDialog.SelectedPath = Settings.Default.InitDir;
            }
        }

        private void buttonSelectDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxDir.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonDo_Click(object sender, EventArgs e)
        {
            this.photoDir = this.textBoxDir.Text.Trim();

            if (String.IsNullOrEmpty(this.photoDir))
            {
                MessageBox.Show("Please select photo folder.", "Select folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBoxDir.Focus();
                return;
            }

            if (!Directory.Exists(this.photoDir))
            {
                MessageBox.Show("The photo folder is not exist.", "Photo folder error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.textBoxDir.Focus();
                return;
            }

            this.SaveSelectDir();

            this.textBoxResult.Clear();

            this.buttonDo.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            this.textBoxResult.Text = "Start...";

            try
            {
                Renamer rn;
                Renamer.Log log = new Renamer.Log(this.AppendNewLineResult);
                bool recursive = this.checkBoxRecursive.Checked;

                if (this.radioButtonUpdatedTime.Checked)
                {
                    rn = new UpdateTimeRenamer(this.photoDir, log, recursive);
                }
                else
                {
                    // parameter 'recursive' is not used so far
                    rn = new ExifToolRenamer(this.appDir, this.photoDir, log, recursive);
                }

                rn.Rename();
            }
            catch (Exception ex)
            {
                this.textBoxResult.Text = "error : ";
                this.AppendNewLineResult(ex.StackTrace);
            }

            this.ScrollResultBoxToBottom();
            this.buttonDo.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void SaveSelectDir()
        {
            try
            {
                DirectoryInfo dirInfo = Directory.GetParent(this.photoDir);
                Settings.Default.InitDir = dirInfo.FullName;
                Settings.Default.Save();
            }
            catch
            {
            }
        }

        private void AppendNewLineResult(String text)
        {
            this.textBoxResult.Text += (Environment.NewLine + text);
        }

        private void ScrollResultBoxToBottom()
        {
            this.textBoxResult.SelectionStart = textBoxResult.Text.Length;
            this.textBoxResult.ScrollToCaret();
        }

        private void buttonArchive_Click(object sender, EventArgs e)
        {
            new ArchiveForm(this.textBoxDir.Text).ShowDialog();
        }
    }
}
