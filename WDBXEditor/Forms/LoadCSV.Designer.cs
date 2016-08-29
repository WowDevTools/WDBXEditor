namespace WDBXEditor
{
    partial class LoadCSV
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadCSV));
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkHeader = new System.Windows.Forms.CheckBox();
            this.radUpdate = new System.Windows.Forms.RadioButton();
            this.radOverride = new System.Windows.Forms.RadioButton();
            this.radNewOnly = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoFixIds = new System.Windows.Forms.RadioButton();
            this.rdoNewest = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(209, 178);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(290, 178);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(37, 13);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(221, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(264, 12);
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
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "File:";
            // 
            // chkHeader
            // 
            this.chkHeader.AutoSize = true;
            this.chkHeader.Checked = true;
            this.chkHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHeader.Location = new System.Drawing.Point(37, 62);
            this.chkHeader.Name = "chkHeader";
            this.chkHeader.Size = new System.Drawing.Size(114, 17);
            this.chkHeader.TabIndex = 6;
            this.chkHeader.Text = "Has Header Row?";
            this.toolTip1.SetToolTip(this.chkHeader, "Will ignore the first row");
            this.chkHeader.UseVisualStyleBackColor = true;
            // 
            // radUpdate
            // 
            this.radUpdate.AutoSize = true;
            this.radUpdate.Location = new System.Drawing.Point(122, 39);
            this.radUpdate.Name = "radUpdate";
            this.radUpdate.Size = new System.Drawing.Size(99, 17);
            this.radUpdate.TabIndex = 4;
            this.radUpdate.Text = "Update Existing";
            this.radUpdate.UseVisualStyleBackColor = true;
            // 
            // radOverride
            // 
            this.radOverride.AutoSize = true;
            this.radOverride.Location = new System.Drawing.Point(227, 39);
            this.radOverride.Name = "radOverride";
            this.radOverride.Size = new System.Drawing.Size(79, 17);
            this.radOverride.TabIndex = 5;
            this.radOverride.Text = "Override All";
            this.radOverride.UseVisualStyleBackColor = true;
            // 
            // radNewOnly
            // 
            this.radNewOnly.AutoSize = true;
            this.radNewOnly.Checked = true;
            this.radNewOnly.Location = new System.Drawing.Point(37, 39);
            this.radNewOnly.Name = "radNewOnly";
            this.radNewOnly.Size = new System.Drawing.Size(79, 17);
            this.radNewOnly.TabIndex = 3;
            this.radNewOnly.TabStop = true;
            this.radNewOnly.Text = "Import New";
            this.radNewOnly.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtFilePath);
            this.panel1.Controls.Add(this.radNewOnly);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.radOverride);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.radUpdate);
            this.panel1.Controls.Add(this.chkHeader);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 160);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoFixIds);
            this.panel2.Controls.Add(this.rdoNewest);
            this.panel2.Location = new System.Drawing.Point(150, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(160, 22);
            this.panel2.TabIndex = 22;
            // 
            // rdoFixIds
            // 
            this.rdoFixIds.AutoSize = true;
            this.rdoFixIds.Checked = true;
            this.rdoFixIds.Location = new System.Drawing.Point(3, 3);
            this.rdoFixIds.Name = "rdoFixIds";
            this.rdoFixIds.Size = new System.Drawing.Size(55, 17);
            this.rdoFixIds.TabIndex = 20;
            this.rdoFixIds.TabStop = true;
            this.rdoFixIds.Text = "Fix Ids";
            this.toolTip1.SetToolTip(this.rdoFixIds, "This will increment all Ids so that there are no duplicates");
            this.rdoFixIds.UseVisualStyleBackColor = true;
            this.rdoFixIds.CheckedChanged += new System.EventHandler(this.chkFixIds_CheckedChanged);
            // 
            // rdoNewest
            // 
            this.rdoNewest.AutoSize = true;
            this.rdoNewest.Location = new System.Drawing.Point(65, 3);
            this.rdoNewest.Name = "rdoNewest";
            this.rdoNewest.Size = new System.Drawing.Size(89, 17);
            this.rdoNewest.TabIndex = 21;
            this.rdoNewest.Text = "Take Newest";
            this.toolTip1.SetToolTip(this.rdoNewest, "This will override previous data if a newer row with the same Id is encountered");
            this.rdoNewest.UseVisualStyleBackColor = true;
            this.rdoNewest.CheckedChanged += new System.EventHandler(this.chkNewest_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 138);
            this.label4.MaximumSize = new System.Drawing.Size(370, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Override All: Replaces all current data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 107);
            this.label3.MaximumSize = new System.Drawing.Size(350, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(349, 26);
            this.label3.TabIndex = 18;
            this.label3.Text = "Update Existing: Imports new records and updates any existing ones that are diffe" +
    "rent";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 89);
            this.label2.MaximumSize = new System.Drawing.Size(200, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Import New: Imports new records only";
            // 
            // LoadCSV
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 208);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(396, 247);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(396, 247);
            this.Name = "LoadCSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CSV Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHeader;
        private System.Windows.Forms.RadioButton radUpdate;
        private System.Windows.Forms.RadioButton radOverride;
        private System.Windows.Forms.RadioButton radNewOnly;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoFixIds;
        private System.Windows.Forms.RadioButton rdoNewest;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
    }
}