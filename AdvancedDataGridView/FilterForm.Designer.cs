
namespace ADGV
{
    partial class FilterForm
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_columnName = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.dgvFilter = new System.Windows.Forms.DataGridView();
            this.Filter = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operator = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.Location = new System.Drawing.Point(329, 186);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(410, 186);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label_columnName
            // 
            this.label_columnName.AutoSize = true;
            this.label_columnName.Location = new System.Drawing.Point(4, 9);
            this.label_columnName.Name = "label_columnName";
            this.label_columnName.Size = new System.Drawing.Size(120, 13);
            this.label_columnName.TabIndex = 2;
            this.label_columnName.Text = "Show rows where value";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // dgvFilter
            // 
            this.dgvFilter.AllowUserToResizeRows = false;
            this.dgvFilter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Filter,
            this.Value,
            this.Operator});
            this.dgvFilter.Location = new System.Drawing.Point(7, 25);
            this.dgvFilter.Name = "dgvFilter";
            this.dgvFilter.Size = new System.Drawing.Size(478, 155);
            this.dgvFilter.TabIndex = 4;
            this.dgvFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvFilter_MouseDown);
            // 
            // Filter
            // 
            this.Filter.FillWeight = 119.5431F;
            this.Filter.HeaderText = "Filter Operation";
            this.Filter.Name = "Filter";
            // 
            // Value
            // 
            this.Value.FillWeight = 119.5431F;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // Operator
            // 
            this.Operator.FillWeight = 60.9137F;
            this.Operator.HeaderText = "Operator";
            this.Operator.Items.AddRange(new object[] {
            "",
            "AND",
            "OR"});
            this.Operator.Name = "Operator";
            // 
            // btnReset
            // 
            this.btnReset.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReset.Location = new System.Drawing.Point(7, 186);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // FormCustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(497, 217);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.dgvFilter);
            this.Controls.Add(this.label_columnName);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(513, 256);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(513, 256);
            this.Name = "FormCustomFilter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FormCustomFilter_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCustomFilter_FormClosing);
            this.Load += new System.EventHandler(this.FormCustomFilter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_columnName;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridView dgvFilter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridViewComboBoxColumn Filter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewComboBoxColumn Operator;
    }
}