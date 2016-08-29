namespace WDBXEditor.Common
{
    partial class DropdownCheckList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbItems = new System.Windows.Forms.CheckedListBox();
            this.cbBox = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbItems
            // 
            this.lbItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbItems.CheckOnClick = true;
            this.lbItems.FormattingEnabled = true;
            this.lbItems.Location = new System.Drawing.Point(0, 21);
            this.lbItems.Name = "lbItems";
            this.lbItems.Size = new System.Drawing.Size(272, 94);
            this.lbItems.TabIndex = 1;
            this.lbItems.Visible = false;
            this.lbItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbItems_ItemCheck);
            // 
            // cbBox
            // 
            this.cbBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbBox.DropDownHeight = 1;
            this.cbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox.DropDownWidth = 1;
            this.cbBox.FormattingEnabled = true;
            this.cbBox.IntegralHeight = false;
            this.cbBox.Items.AddRange(new object[] {
            "[All]"});
            this.cbBox.Location = new System.Drawing.Point(0, 0);
            this.cbBox.Name = "cbBox";
            this.cbBox.Size = new System.Drawing.Size(214, 21);
            this.cbBox.TabIndex = 0;
            this.cbBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbBox_DrawItem);
            this.cbBox.DropDown += new System.EventHandler(this.cbBox_DropDown);
            this.cbBox.DropDownClosed += new System.EventHandler(this.cbBox_DropDownClosed);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmpty.Image = global::WDBXEditor.Properties.Resources.hide;
            this.btnEmpty.Location = new System.Drawing.Point(217, -1);
            this.btnEmpty.Margin = new System.Windows.Forms.Padding(0);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(26, 23);
            this.btnEmpty.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnEmpty, "Hide Empty");
            this.btnEmpty.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Image = global::WDBXEditor.Properties.Resources.close;
            this.btnReset.Location = new System.Drawing.Point(246, -1);
            this.btnReset.Margin = new System.Windows.Forms.Padding(0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(26, 23);
            this.btnReset.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnReset, "Reset");
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // DropdownCheckList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnEmpty);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lbItems);
            this.Controls.Add(this.cbBox);
            this.Name = "DropdownCheckList";
            this.Size = new System.Drawing.Size(272, 115);
            this.EnabledChanged += new System.EventHandler(this.DropdownCheckList_EnabledChanged);
            this.Leave += new System.EventHandler(this.DropdownCheckList_Leave);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckedListBox lbItems;
        private System.Windows.Forms.ComboBox cbBox;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnEmpty;
    }
}
