
namespace ADGV
{
    partial class ColumnMenu
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
            this.sortASCMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortDESCMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelSortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.cancelFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customFilterLastFiltersListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexDisplayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendLayout();

            //
            // MenuStrip
            //
            this.AutoSize = false;
            this.Size = new System.Drawing.Size(27, 186);            
            //
            // sortASCMenuItem
            //
            this.sortASCMenuItem.Name = "sortASCMenuItem";
            this.sortASCMenuItem.AutoSize = false;
            this.sortASCMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            //
            // sortDESCMenuItem
            //
            this.sortDESCMenuItem.Name = "sortDESCMenuItem";
            this.sortDESCMenuItem.AutoSize = false;
            this.sortDESCMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            //
            // cancelSortMenuItem
            //
            this.cancelSortMenuItem.Name = "cancelSortMenuItem";
            this.cancelSortMenuItem.Enabled = false;
            this.cancelSortMenuItem.AutoSize = false;
            this.cancelSortMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            this.cancelSortMenuItem.Text = _textStrings["CLEARSORT"].ToString();
            //
            // toolStripSeparator1MenuItem
            //
            this.toolStripSeparator1MenuItem.Name = "toolStripSeparator1MenuItem";
            this.toolStripSeparator1MenuItem.Size = new System.Drawing.Size(Width - 4, 6);
            //
            // toolStripSeparator2MenuItem
            //
            this.toolStripSeparator2MenuItem.Name = "toolStripSeparator2MenuItem";
            this.toolStripSeparator2MenuItem.Size = new System.Drawing.Size(Width - 4, 6);
            //
            // cancelFilterMenuItem
            //
            this.cancelFilterMenuItem.Name = "cancelFilterMenuItem";
            this.cancelFilterMenuItem.Enabled = false;
            this.cancelFilterMenuItem.AutoSize = false;
            this.cancelFilterMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            this.cancelFilterMenuItem.Text = _textStrings["CLEARFILTER"].ToString();
            //
            // customFilterLastFiltersListMenuItem
            //
            this.customFilterLastFiltersListMenuItem.Name = "customFilterLastFiltersListMenuItem";
            this.customFilterLastFiltersListMenuItem.AutoSize = false;
            this.customFilterLastFiltersListMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            //
            // hexDisplayMenuItem
            //
            this.hexDisplayMenuItem.Name = "hexDisplayMenuItem";
            this.hexDisplayMenuItem.AutoSize = true;
            this.hexDisplayMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            this.hexDisplayMenuItem.Text = "Display as Hex";
            //
            // hexDisplayMenuItem
            //
            this.hideMenuItem.Name = "hideMenuItem";
            this.hideMenuItem.AutoSize = true;
            this.hideMenuItem.Size = new System.Drawing.Size(Width - 1, 22);
            this.hideMenuItem.Text = "Hide";

            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                hexDisplayMenuItem,
                toolStripSeparator2MenuItem,
                sortASCMenuItem,
                sortDESCMenuItem,
                cancelSortMenuItem,
                toolStripSeparator1MenuItem,
                customFilterLastFiltersListMenuItem,
                cancelFilterMenuItem,                
            });
            
            this.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(MenuStrip_Closed);
            this.LostFocus += new System.EventHandler(MenuStrip_LostFocus);
            this.cancelFilterMenuItem.Click += new System.EventHandler(cancelFilterMenuItem_Click);
            this.cancelFilterMenuItem.MouseEnter += new System.EventHandler(cancelFilterMenuItem_MouseEnter);
            this.sortASCMenuItem.Click += new System.EventHandler(sortASCMenuItem_Click);
            this.sortASCMenuItem.MouseEnter += new System.EventHandler(sortASCMenuItem_MouseEnter);
            this.sortDESCMenuItem.Click += new System.EventHandler(sortDESCMenuItem_Click);
            this.sortDESCMenuItem.MouseEnter += new System.EventHandler(sortDESCMenuItem_MouseEnter);
            this.cancelSortMenuItem.Click += new System.EventHandler(cancelSortMenuItem_Click);
            this.cancelSortMenuItem.MouseEnter += new System.EventHandler(cancelSortMenuItem_MouseEnter);
            this.customFilterLastFiltersListMenuItem.Click += new System.EventHandler(customFilterMenuItem_Click);
            this.hideMenuItem.Click += new System.EventHandler(hideMenuItem_Click);
            this.hexDisplayMenuItem.Click += new System.EventHandler(hexDisplayMenuItem_Click);


            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem sortASCMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortDESCMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelSortMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelFilterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customFilterLastFiltersListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexDisplayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideMenuItem;
    }
}
