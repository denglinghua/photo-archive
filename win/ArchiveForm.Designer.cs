namespace PhotoRenamer
{
    partial class ArchiveForm
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
            this.checkBoxSubFolders = new System.Windows.Forms.CheckBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.textBoxArchiveDir = new System.Windows.Forms.TextBox();
            this.labelDest = new System.Windows.Forms.Label();
            this.textBoxSourceDir = new System.Windows.Forms.TextBox();
            this.labelSourceDir = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxSubFolders
            // 
            this.checkBoxSubFolders.AutoSize = true;
            this.checkBoxSubFolders.Location = new System.Drawing.Point(493, 22);
            this.checkBoxSubFolders.Name = "checkBoxSubFolders";
            this.checkBoxSubFolders.Size = new System.Drawing.Size(90, 16);
            this.checkBoxSubFolders.TabIndex = 12;
            this.checkBoxSubFolders.Text = "Sub Folders";
            this.checkBoxSubFolders.UseVisualStyleBackColor = true;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(81, 100);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 11;
            this.buttonGo.Text = "Archive";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonArchive_Click);
            // 
            // textBoxArchiveDir
            // 
            this.textBoxArchiveDir.Location = new System.Drawing.Point(83, 60);
            this.textBoxArchiveDir.Name = "textBoxArchiveDir";
            this.textBoxArchiveDir.Size = new System.Drawing.Size(388, 21);
            this.textBoxArchiveDir.TabIndex = 10;
            // 
            // labelDest
            // 
            this.labelDest.AutoSize = true;
            this.labelDest.Location = new System.Drawing.Point(18, 63);
            this.labelDest.Name = "labelDest";
            this.labelDest.Size = new System.Drawing.Size(47, 12);
            this.labelDest.TabIndex = 9;
            this.labelDest.Text = "Archive";
            // 
            // textBoxSourceDir
            // 
            this.textBoxSourceDir.Location = new System.Drawing.Point(83, 19);
            this.textBoxSourceDir.Name = "textBoxSourceDir";
            this.textBoxSourceDir.Size = new System.Drawing.Size(388, 21);
            this.textBoxSourceDir.TabIndex = 8;
            // 
            // labelSourceDir
            // 
            this.labelSourceDir.AutoSize = true;
            this.labelSourceDir.Location = new System.Drawing.Point(18, 22);
            this.labelSourceDir.Name = "labelSourceDir";
            this.labelSourceDir.Size = new System.Drawing.Size(41, 12);
            this.labelSourceDir.TabIndex = 7;
            this.labelSourceDir.Text = "Source";
            // 
            // ArchiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 170);
            this.Controls.Add(this.checkBoxSubFolders);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.textBoxArchiveDir);
            this.Controls.Add(this.labelDest);
            this.Controls.Add(this.textBoxSourceDir);
            this.Controls.Add(this.labelSourceDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "ArchiveForm";
            this.Text = "Archive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSubFolders;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxArchiveDir;
        private System.Windows.Forms.Label labelDest;
        private System.Windows.Forms.TextBox textBoxSourceDir;
        private System.Windows.Forms.Label labelSourceDir;
    }
}