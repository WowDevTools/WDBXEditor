using WDBXEditor.Common;

namespace WDBXEditor
{
    partial class LoadMPQ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadMPQ));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstFiles = new WDBXEditor.Common.BufferedListBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.progressBar = new WDBXEditor.Common.AutoProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstFiles);
            this.panel1.Controls.Add(this.txtFilePath);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 285);
            this.panel1.TabIndex = 14;
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(8, 39);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFiles.Size = new System.Drawing.Size(336, 238);
            this.lstFiles.Sorted = true;
            this.lstFiles.TabIndex = 8;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(37, 9);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(221, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(264, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "File:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "MPQ files (*.mpq)|*.mpq";
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(211, 303);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 15;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(292, 303);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 304);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(185, 21);
            this.progressBar.TabIndex = 17;
            // 
            // LoadMPQ
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 338);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadMPQ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load From MPQ";
            this.Load += new System.EventHandler(this.LoadMPQ_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private BufferedListBox lstFiles;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnClose;
        private Common.AutoProgressBar progressBar;
    }
}