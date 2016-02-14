namespace PhotoRenamer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelDir = new System.Windows.Forms.Label();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.buttonSelectDir = new System.Windows.Forms.Button();
            this.checkBoxRecursive = new System.Windows.Forms.CheckBox();
            this.buttonDo = new System.Windows.Forms.Button();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.radioButtonExif = new System.Windows.Forms.RadioButton();
            this.radioButtonUpdatedTime = new System.Windows.Forms.RadioButton();
            this.buttonArchive = new System.Windows.Forms.Button();
            this.groupBoxResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDir
            // 
            this.labelDir.AutoSize = true;
            this.labelDir.Location = new System.Drawing.Point(9, 21);
            this.labelDir.Name = "labelDir";
            this.labelDir.Size = new System.Drawing.Size(77, 12);
            this.labelDir.TabIndex = 0;
            this.labelDir.Text = "Photo Folder";
            // 
            // textBoxDir
            // 
            this.textBoxDir.Location = new System.Drawing.Point(102, 17);
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.Size = new System.Drawing.Size(523, 21);
            this.textBoxDir.TabIndex = 1;
            // 
            // buttonSelectDir
            // 
            this.buttonSelectDir.Location = new System.Drawing.Point(637, 17);
            this.buttonSelectDir.Name = "buttonSelectDir";
            this.buttonSelectDir.Size = new System.Drawing.Size(72, 23);
            this.buttonSelectDir.TabIndex = 2;
            this.buttonSelectDir.Text = "Select";
            this.buttonSelectDir.UseVisualStyleBackColor = true;
            this.buttonSelectDir.Click += new System.EventHandler(this.buttonSelectDir_Click);
            // 
            // checkBoxRecursive
            // 
            this.checkBoxRecursive.AutoSize = true;
            this.checkBoxRecursive.Location = new System.Drawing.Point(487, 58);
            this.checkBoxRecursive.Name = "checkBoxRecursive";
            this.checkBoxRecursive.Size = new System.Drawing.Size(138, 16);
            this.checkBoxRecursive.TabIndex = 5;
            this.checkBoxRecursive.Text = "Include Sub Folders";
            this.checkBoxRecursive.UseVisualStyleBackColor = true;
            // 
            // buttonDo
            // 
            this.buttonDo.Location = new System.Drawing.Point(93, 119);
            this.buttonDo.Name = "buttonDo";
            this.buttonDo.Size = new System.Drawing.Size(84, 23);
            this.buttonDo.TabIndex = 6;
            this.buttonDo.Text = "Rename";
            this.buttonDo.UseVisualStyleBackColor = true;
            this.buttonDo.Click += new System.EventHandler(this.buttonDo_Click);
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Controls.Add(this.textBoxResult);
            this.groupBoxResult.Location = new System.Drawing.Point(21, 148);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(688, 436);
            this.groupBoxResult.TabIndex = 5;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "Result";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(16, 22);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResult.Size = new System.Drawing.Size(660, 400);
            this.textBoxResult.TabIndex = 7;
            this.textBoxResult.WordWrap = false;
            // 
            // radioButtonExif
            // 
            this.radioButtonExif.AutoSize = true;
            this.radioButtonExif.Checked = true;
            this.radioButtonExif.Location = new System.Drawing.Point(103, 58);
            this.radioButtonExif.Name = "radioButtonExif";
            this.radioButtonExif.Size = new System.Drawing.Size(65, 16);
            this.radioButtonExif.TabIndex = 3;
            this.radioButtonExif.TabStop = true;
            this.radioButtonExif.Text = "By Exif";
            this.radioButtonExif.UseVisualStyleBackColor = true;
            // 
            // radioButtonUpdatedTime
            // 
            this.radioButtonUpdatedTime.AutoSize = true;
            this.radioButtonUpdatedTime.Location = new System.Drawing.Point(225, 58);
            this.radioButtonUpdatedTime.Name = "radioButtonUpdatedTime";
            this.radioButtonUpdatedTime.Size = new System.Drawing.Size(113, 16);
            this.radioButtonUpdatedTime.TabIndex = 4;
            this.radioButtonUpdatedTime.Text = "By Updated Time";
            this.radioButtonUpdatedTime.UseVisualStyleBackColor = true;
            // 
            // buttonArchive
            // 
            this.buttonArchive.Location = new System.Drawing.Point(583, 119);
            this.buttonArchive.Name = "buttonArchive";
            this.buttonArchive.Size = new System.Drawing.Size(126, 23);
            this.buttonArchive.TabIndex = 7;
            this.buttonArchive.Text = "Archive By Month";
            this.buttonArchive.UseVisualStyleBackColor = true;
            this.buttonArchive.Click += new System.EventHandler(this.buttonArchive_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 596);
            this.Controls.Add(this.buttonArchive);
            this.Controls.Add(this.radioButtonUpdatedTime);
            this.Controls.Add(this.radioButtonExif);
            this.Controls.Add(this.groupBoxResult);
            this.Controls.Add(this.buttonDo);
            this.Controls.Add(this.checkBoxRecursive);
            this.Controls.Add(this.buttonSelectDir);
            this.Controls.Add(this.textBoxDir);
            this.Controls.Add(this.labelDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Rename Photo (V{0})";
            this.groupBoxResult.ResumeLayout(false);
            this.groupBoxResult.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDir;
        private System.Windows.Forms.TextBox textBoxDir;
        private System.Windows.Forms.Button buttonSelectDir;
        private System.Windows.Forms.CheckBox checkBoxRecursive;
        private System.Windows.Forms.Button buttonDo;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.RadioButton radioButtonExif;
        private System.Windows.Forms.RadioButton radioButtonUpdatedTime;
        private System.Windows.Forms.Button buttonArchive;
    }
}

