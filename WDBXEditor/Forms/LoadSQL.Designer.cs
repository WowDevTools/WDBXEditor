namespace WDBXEditor
{
    partial class LoadSQL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadSQL));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ddlDatabases = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ddlTable = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.radNewOnly = new System.Windows.Forms.RadioButton();
            this.radOverride = new System.Windows.Forms.RadioButton();
            this.radUpdate = new System.Windows.Forms.RadioButton();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(67, 11);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(275, 20);
            this.txtHost.TabIndex = 1;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(67, 37);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(275, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "3306";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(67, 63);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(275, 20);
            this.txtUser.TabIndex = 3;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(67, 89);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(275, 20);
            this.txtPass.TabIndex = 4;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Database:";
            // 
            // ddlDatabases
            // 
            this.ddlDatabases.Enabled = false;
            this.ddlDatabases.FormattingEnabled = true;
            this.ddlDatabases.Location = new System.Drawing.Point(67, 115);
            this.ddlDatabases.Name = "ddlDatabases";
            this.ddlDatabases.Size = new System.Drawing.Size(202, 21);
            this.ddlDatabases.TabIndex = 6;
            this.ddlDatabases.SelectedIndexChanged += new System.EventHandler(this.ddlDatabases_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(275, 115);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 21);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Connect";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ddlTable);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.radNewOnly);
            this.panel1.Controls.Add(this.radOverride);
            this.panel1.Controls.Add(this.radUpdate);
            this.panel1.Controls.Add(this.txtUser);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ddlDatabases);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPass);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtHost);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 257);
            this.panel1.TabIndex = 11;
            // 
            // ddlTable
            // 
            this.ddlTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTable.FormattingEnabled = true;
            this.ddlTable.Location = new System.Drawing.Point(66, 142);
            this.ddlTable.Name = "ddlTable";
            this.ddlTable.Size = new System.Drawing.Size(203, 21);
            this.ddlTable.TabIndex = 7;
            this.ddlTable.SelectedIndexChanged += new System.EventHandler(this.ddlTable_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 145);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Table:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 238);
            this.label6.MaximumSize = new System.Drawing.Size(370, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Override All: Replaces all data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 207);
            this.label7.MaximumSize = new System.Drawing.Size(350, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(349, 26);
            this.label7.TabIndex = 24;
            this.label7.Text = "Update Existing: Imports new records and updates any existing ones that are diffe" +
    "rent";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 189);
            this.label8.MaximumSize = new System.Drawing.Size(200, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Import New: Imports new records only";
            // 
            // radNewOnly
            // 
            this.radNewOnly.AutoSize = true;
            this.radNewOnly.Checked = true;
            this.radNewOnly.Location = new System.Drawing.Point(66, 169);
            this.radNewOnly.Name = "radNewOnly";
            this.radNewOnly.Size = new System.Drawing.Size(79, 17);
            this.radNewOnly.TabIndex = 8;
            this.radNewOnly.TabStop = true;
            this.radNewOnly.Text = "Import New";
            this.radNewOnly.UseVisualStyleBackColor = true;
            // 
            // radOverride
            // 
            this.radOverride.AutoSize = true;
            this.radOverride.Location = new System.Drawing.Point(256, 169);
            this.radOverride.Name = "radOverride";
            this.radOverride.Size = new System.Drawing.Size(79, 17);
            this.radOverride.TabIndex = 10;
            this.radOverride.Text = "Override All";
            this.radOverride.UseVisualStyleBackColor = true;
            // 
            // radUpdate
            // 
            this.radUpdate.AutoSize = true;
            this.radUpdate.Location = new System.Drawing.Point(151, 169);
            this.radUpdate.Name = "radUpdate";
            this.radUpdate.Size = new System.Drawing.Size(99, 17);
            this.radUpdate.TabIndex = 9;
            this.radUpdate.Text = "Update Existing";
            this.radUpdate.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(212, 275);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(293, 275);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // LoadSQL
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 303);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(396, 342);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(396, 342);
            this.Name = "LoadSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Settings";
            this.Load += new System.EventHandler(this.LoadSQL_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ddlDatabases;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radNewOnly;
        private System.Windows.Forms.RadioButton radOverride;
        private System.Windows.Forms.RadioButton radUpdate;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox ddlTable;
        private System.Windows.Forms.Label label9;
    }
}