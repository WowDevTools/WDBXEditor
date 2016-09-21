namespace WDBXEditor.Forms
{
    partial class PlayerLocation 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerLocation));
            this.label1 = new System.Windows.Forms.Label();
            this.cbProcessSelector = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGetLoc = new System.Windows.Forms.GroupBox();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.btnGetPos = new System.Windows.Forms.Button();
            this.txtCurYPos = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCurXPos = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCurZPos = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCurMap = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpOffsets = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPosX = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMapId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNextObject = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLocalGUID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFirstObject = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtObjectManager = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClientConnection = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUntarget = new System.Windows.Forms.Button();
            this.btnTarget = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBuildSelector = new System.Windows.Forms.ComboBox();
            this.tmrLoop = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.btnGetLoc.SuspendLayout();
            this.grpOffsets.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Process";
            // 
            // cbProcessSelector
            // 
            this.cbProcessSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProcessSelector.FormattingEnabled = true;
            this.cbProcessSelector.Location = new System.Drawing.Point(54, 3);
            this.cbProcessSelector.Name = "cbProcessSelector";
            this.cbProcessSelector.Size = new System.Drawing.Size(126, 21);
            this.cbProcessSelector.TabIndex = 1;
            this.cbProcessSelector.SelectedIndexChanged += new System.EventHandler(this.cbProcessSelector_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnGetLoc);
            this.panel1.Controls.Add(this.grpOffsets);
            this.panel1.Controls.Add(this.btnUntarget);
            this.panel1.Controls.Add(this.btnTarget);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbBuildSelector);
            this.panel1.Controls.Add(this.cbProcessSelector);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(454, 292);
            this.panel1.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::WDBXEditor.Properties.Resources.reload;
            this.btnRefresh.Location = new System.Drawing.Point(186, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(29, 23);
            this.btnRefresh.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnRefresh, "Reload processes");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnGetLoc
            // 
            this.btnGetLoc.Controls.Add(this.chkAuto);
            this.btnGetLoc.Controls.Add(this.btnGetPos);
            this.btnGetLoc.Controls.Add(this.txtCurYPos);
            this.btnGetLoc.Controls.Add(this.label14);
            this.btnGetLoc.Controls.Add(this.txtCurXPos);
            this.btnGetLoc.Controls.Add(this.label13);
            this.btnGetLoc.Controls.Add(this.txtCurZPos);
            this.btnGetLoc.Controls.Add(this.label12);
            this.btnGetLoc.Controls.Add(this.txtCurMap);
            this.btnGetLoc.Controls.Add(this.label9);
            this.btnGetLoc.Location = new System.Drawing.Point(212, 30);
            this.btnGetLoc.Name = "btnGetLoc";
            this.btnGetLoc.Size = new System.Drawing.Size(239, 154);
            this.btnGetLoc.TabIndex = 7;
            this.btnGetLoc.TabStop = false;
            this.btnGetLoc.Text = "Location";
            // 
            // chkAuto
            // 
            this.chkAuto.AutoSize = true;
            this.chkAuto.Location = new System.Drawing.Point(57, 127);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(86, 17);
            this.chkAuto.TabIndex = 29;
            this.chkAuto.Text = "Auto Update";
            this.toolTip1.SetToolTip(this.chkAuto, "Poll player location");
            this.chkAuto.UseVisualStyleBackColor = true;
            this.chkAuto.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
            // 
            // btnGetPos
            // 
            this.btnGetPos.Location = new System.Drawing.Point(150, 123);
            this.btnGetPos.Name = "btnGetPos";
            this.btnGetPos.Size = new System.Drawing.Size(83, 23);
            this.btnGetPos.TabIndex = 28;
            this.btnGetPos.Text = "Get Location";
            this.btnGetPos.UseVisualStyleBackColor = true;
            this.btnGetPos.Click += new System.EventHandler(this.btnGetPos_Click);
            // 
            // txtCurYPos
            // 
            this.txtCurYPos.Location = new System.Drawing.Point(57, 71);
            this.txtCurYPos.MaxLength = 14;
            this.txtCurYPos.Name = "txtCurYPos";
            this.txtCurYPos.Size = new System.Drawing.Size(176, 20);
            this.txtCurYPos.TabIndex = 26;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "Y Coord";
            // 
            // txtCurXPos
            // 
            this.txtCurXPos.Location = new System.Drawing.Point(57, 45);
            this.txtCurXPos.MaxLength = 14;
            this.txtCurXPos.Name = "txtCurXPos";
            this.txtCurXPos.Size = new System.Drawing.Size(176, 20);
            this.txtCurXPos.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "X Coord";
            // 
            // txtCurZPos
            // 
            this.txtCurZPos.Location = new System.Drawing.Point(57, 97);
            this.txtCurZPos.MaxLength = 14;
            this.txtCurZPos.Name = "txtCurZPos";
            this.txtCurZPos.Size = new System.Drawing.Size(176, 20);
            this.txtCurZPos.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 103);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Z Coord";
            // 
            // txtCurMap
            // 
            this.txtCurMap.Location = new System.Drawing.Point(57, 19);
            this.txtCurMap.MaxLength = 14;
            this.txtCurMap.Name = "txtCurMap";
            this.txtCurMap.Size = new System.Drawing.Size(176, 20);
            this.txtCurMap.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Map Id";
            // 
            // grpOffsets
            // 
            this.grpOffsets.Controls.Add(this.btnDelete);
            this.grpOffsets.Controls.Add(this.btnSave);
            this.grpOffsets.Controls.Add(this.txtPosX);
            this.grpOffsets.Controls.Add(this.label11);
            this.grpOffsets.Controls.Add(this.txtGUID);
            this.grpOffsets.Controls.Add(this.label10);
            this.grpOffsets.Controls.Add(this.txtMapId);
            this.grpOffsets.Controls.Add(this.label8);
            this.grpOffsets.Controls.Add(this.txtNextObject);
            this.grpOffsets.Controls.Add(this.label7);
            this.grpOffsets.Controls.Add(this.txtLocalGUID);
            this.grpOffsets.Controls.Add(this.label6);
            this.grpOffsets.Controls.Add(this.txtFirstObject);
            this.grpOffsets.Controls.Add(this.label5);
            this.grpOffsets.Controls.Add(this.txtObjectManager);
            this.grpOffsets.Controls.Add(this.label4);
            this.grpOffsets.Controls.Add(this.txtClientConnection);
            this.grpOffsets.Controls.Add(this.label3);
            this.grpOffsets.Location = new System.Drawing.Point(6, 30);
            this.grpOffsets.Name = "grpOffsets";
            this.grpOffsets.Size = new System.Drawing.Size(200, 259);
            this.grpOffsets.TabIndex = 6;
            this.grpOffsets.TabStop = false;
            this.grpOffsets.Text = "Offsets";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(38, 232);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 27;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(119, 232);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 26;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPosX
            // 
            this.txtPosX.Location = new System.Drawing.Point(103, 201);
            this.txtPosX.MaxLength = 14;
            this.txtPosX.Name = "txtPosX";
            this.txtPosX.Size = new System.Drawing.Size(91, 20);
            this.txtPosX.TabIndex = 20;
            this.txtPosX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Position X";
            // 
            // txtGUID
            // 
            this.txtGUID.Location = new System.Drawing.Point(103, 175);
            this.txtGUID.MaxLength = 14;
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(91, 20);
            this.txtGUID.TabIndex = 18;
            this.txtGUID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Guid";
            // 
            // txtMapId
            // 
            this.txtMapId.Location = new System.Drawing.Point(103, 149);
            this.txtMapId.MaxLength = 14;
            this.txtMapId.Name = "txtMapId";
            this.txtMapId.Size = new System.Drawing.Size(91, 20);
            this.txtMapId.TabIndex = 16;
            this.txtMapId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Map Id";
            // 
            // txtNextObject
            // 
            this.txtNextObject.Location = new System.Drawing.Point(103, 123);
            this.txtNextObject.MaxLength = 14;
            this.txtNextObject.Name = "txtNextObject";
            this.txtNextObject.Size = new System.Drawing.Size(91, 20);
            this.txtNextObject.TabIndex = 14;
            this.txtNextObject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Next Object";
            // 
            // txtLocalGUID
            // 
            this.txtLocalGUID.Location = new System.Drawing.Point(103, 97);
            this.txtLocalGUID.MaxLength = 14;
            this.txtLocalGUID.Name = "txtLocalGUID";
            this.txtLocalGUID.Size = new System.Drawing.Size(91, 20);
            this.txtLocalGUID.TabIndex = 12;
            this.txtLocalGUID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Local GUID";
            // 
            // txtFirstObject
            // 
            this.txtFirstObject.Location = new System.Drawing.Point(103, 71);
            this.txtFirstObject.MaxLength = 14;
            this.txtFirstObject.Name = "txtFirstObject";
            this.txtFirstObject.Size = new System.Drawing.Size(91, 20);
            this.txtFirstObject.TabIndex = 10;
            this.txtFirstObject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "First Object";
            // 
            // txtObjectManager
            // 
            this.txtObjectManager.Location = new System.Drawing.Point(103, 45);
            this.txtObjectManager.MaxLength = 14;
            this.txtObjectManager.Name = "txtObjectManager";
            this.txtObjectManager.Size = new System.Drawing.Size(91, 20);
            this.txtObjectManager.TabIndex = 8;
            this.txtObjectManager.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Object Manager";
            // 
            // txtClientConnection
            // 
            this.txtClientConnection.Location = new System.Drawing.Point(103, 19);
            this.txtClientConnection.MaxLength = 14;
            this.txtClientConnection.Name = "txtClientConnection";
            this.txtClientConnection.Size = new System.Drawing.Size(91, 20);
            this.txtClientConnection.TabIndex = 7;
            this.txtClientConnection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Number_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Client Connection";
            // 
            // btnUntarget
            // 
            this.btnUntarget.Enabled = false;
            this.btnUntarget.Image = global::WDBXEditor.Properties.Resources.close;
            this.btnUntarget.Location = new System.Drawing.Point(422, 2);
            this.btnUntarget.Name = "btnUntarget";
            this.btnUntarget.Size = new System.Drawing.Size(29, 23);
            this.btnUntarget.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnUntarget, "Detach from process");
            this.btnUntarget.UseVisualStyleBackColor = true;
            this.btnUntarget.Click += new System.EventHandler(this.btnUntarget_Click);
            // 
            // btnTarget
            // 
            this.btnTarget.Enabled = false;
            this.btnTarget.Image = global::WDBXEditor.Properties.Resources.target;
            this.btnTarget.Location = new System.Drawing.Point(387, 2);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(29, 23);
            this.btnTarget.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnTarget, "Attach to process");
            this.btnTarget.UseVisualStyleBackColor = true;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Build";
            // 
            // cbBuildSelector
            // 
            this.cbBuildSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildSelector.FormattingEnabled = true;
            this.cbBuildSelector.Location = new System.Drawing.Point(257, 3);
            this.cbBuildSelector.Name = "cbBuildSelector";
            this.cbBuildSelector.Size = new System.Drawing.Size(124, 21);
            this.cbBuildSelector.TabIndex = 2;
            this.cbBuildSelector.SelectedIndexChanged += new System.EventHandler(this.cbBuildSelector_SelectedIndexChanged);
            // 
            // tmrLoop
            // 
            this.tmrLoop.Interval = 1000;
            this.tmrLoop.Tick += new System.EventHandler(this.tmrLoop_Tick);
            // 
            // PlayerLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 316);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlayerLocation";
            this.Text = "Player Location Reader";
            this.Activated += new System.EventHandler(this.PlayerLocation_Activated);
            this.Deactivate += new System.EventHandler(this.PlayerLocation_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayerLocation_FormClosing);
            this.Load += new System.EventHandler(this.PlayerLocation_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.btnGetLoc.ResumeLayout(false);
            this.btnGetLoc.PerformLayout();
            this.grpOffsets.ResumeLayout(false);
            this.grpOffsets.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProcessSelector;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBuildSelector;
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.Button btnUntarget;
        private System.Windows.Forms.GroupBox grpOffsets;
        private System.Windows.Forms.TextBox txtClientConnection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPosX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtGUID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMapId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNextObject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLocalGUID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFirstObject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtObjectManager;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox btnGetLoc;
        private System.Windows.Forms.Button btnGetPos;
        private System.Windows.Forms.TextBox txtCurYPos;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtCurXPos;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCurZPos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCurMap;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer tmrLoop;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}