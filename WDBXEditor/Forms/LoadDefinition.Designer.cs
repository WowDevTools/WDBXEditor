using WDBXEditor.Common;

namespace WDBXEditor
{
    partial class LoadDefinition
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
            this.lbDefinitions = new WDBXEditor.Common.BufferedListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbDefinitions
            // 
            this.lbDefinitions.FormattingEnabled = true;
            this.lbDefinitions.Location = new System.Drawing.Point(12, 28);
            this.lbDefinitions.Name = "lbDefinitions";
            this.lbDefinitions.Size = new System.Drawing.Size(260, 199);
            this.lbDefinitions.TabIndex = 0;
            this.lbDefinitions.SelectedValueChanged += new System.EventHandler(this.lbDefinitions_SelectedValueChanged);
            this.lbDefinitions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbDefinitions_MouseDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(197, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(116, 230);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "XML Files|*.xml";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Build numbers will be limited to available options";
            // 
            // LoadDefinition
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbDefinitions);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "LoadDefinition";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Definition";
            this.Load += new System.EventHandler(this.LoadDefinition_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BufferedListBox lbDefinitions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label3;
    }
}