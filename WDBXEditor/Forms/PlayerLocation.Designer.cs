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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerLocation));
            this.label1 = new System.Windows.Forms.Label();
            this.cbProcessSelector = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUntarget = new System.Windows.Forms.Button();
            this.btnTarget = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBuildSelector = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
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
            this.cbProcessSelector.Size = new System.Drawing.Size(135, 21);
            this.cbProcessSelector.TabIndex = 1;
            this.cbProcessSelector.SelectedIndexChanged += new System.EventHandler(this.cbProcessSelector_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnUntarget);
            this.panel1.Controls.Add(this.btnTarget);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbBuildSelector);
            this.panel1.Controls.Add(this.cbProcessSelector);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 297);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(6, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 264);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Offsets";
            // 
            // btnUntarget
            // 
            this.btnUntarget.Image = global::WDBXEditor.Properties.Resources.close;
            this.btnUntarget.Location = new System.Drawing.Point(393, 2);
            this.btnUntarget.Name = "btnUntarget";
            this.btnUntarget.Size = new System.Drawing.Size(29, 23);
            this.btnUntarget.TabIndex = 5;
            this.btnUntarget.UseVisualStyleBackColor = true;
            // 
            // btnTarget
            // 
            this.btnTarget.Image = global::WDBXEditor.Properties.Resources.target;
            this.btnTarget.Location = new System.Drawing.Point(358, 2);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(29, 23);
            this.btnTarget.TabIndex = 4;
            this.btnTarget.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Build";
            // 
            // cbBuildSelector
            // 
            this.cbBuildSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildSelector.FormattingEnabled = true;
            this.cbBuildSelector.Location = new System.Drawing.Point(231, 3);
            this.cbBuildSelector.Name = "cbBuildSelector";
            this.cbBuildSelector.Size = new System.Drawing.Size(121, 21);
            this.cbBuildSelector.TabIndex = 2;
            // 
            // PlayerLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 321);
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
        private System.Windows.Forms.GroupBox groupBox1;
    }
}