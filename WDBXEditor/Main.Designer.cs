using ADGV;
using WDBXEditor.Common;

namespace WDBXEditor
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbBuild = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbFiles = new WDBXEditor.Common.BufferedListBox();
            this.filecontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.progressBar = new WDBXEditor.Common.AutoProgressBar();
            this.txtStats = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCurrentCell = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurDefinition = new System.Windows.Forms.TextBox();
            this.txtCurEntry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFromMPQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFromCASCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toSQLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toMPQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDefinitionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wotLKItemFixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wdb5ParserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gotoIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnFilter = new WDBXEditor.Common.DropdownCheckList();
            this.advancedDataGridView = new ADGV.AdvancedDataGridView();
            this.gbFiles.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.filecontextMenuStrip.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // gbFiles
            // 
            this.gbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiles.Controls.Add(this.gbFilter);
            this.gbFiles.Controls.Add(this.lbFiles);
            this.gbFiles.Location = new System.Drawing.Point(12, 392);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(684, 197);
            this.gbFiles.TabIndex = 1;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            // 
            // gbFilter
            // 
            this.gbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFilter.Controls.Add(this.btnReset);
            this.gbFilter.Controls.Add(this.cbBuild);
            this.gbFilter.Controls.Add(this.label6);
            this.gbFilter.Controls.Add(this.txtFilter);
            this.gbFilter.Controls.Add(this.label7);
            this.gbFilter.Location = new System.Drawing.Point(456, 19);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(222, 172);
            this.gbFilter.TabIndex = 10;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(146, 72);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(70, 23);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbBuild
            // 
            this.cbBuild.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuild.FormattingEnabled = true;
            this.cbBuild.Location = new System.Drawing.Point(47, 45);
            this.cbBuild.Name = "cbBuild";
            this.cbBuild.Size = new System.Drawing.Size(169, 21);
            this.cbBuild.TabIndex = 4;
            this.cbBuild.SelectedIndexChanged += new System.EventHandler(this.cbBuild_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Build";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(47, 19);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(169, 20);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Filter";
            // 
            // lbFiles
            // 
            this.lbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(6, 19);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(444, 173);
            this.lbFiles.Sorted = true;
            this.lbFiles.TabIndex = 1;
            this.lbFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseDoubleClick);
            this.lbFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbFiles_MouseDown);
            // 
            // filecontextMenuStrip
            // 
            this.filecontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem1,
            this.closeToolStripMenuItem1});
            this.filecontextMenuStrip.Name = "filecontextMenuStrip";
            this.filecontextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Image = global::WDBXEditor.Properties.Resources.sqlfile;
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.editToolStripMenuItem1.Text = "Edit";
            this.editToolStripMenuItem1.Click += new System.EventHandler(this.editToolStripMenuItem1_Click);
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem1.Image = global::WDBXEditor.Properties.Resources.close;
            this.closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem1.Text = "Close";
            this.closeToolStripMenuItem1.Click += new System.EventHandler(this.closeToolStripMenuItem1_Click);
            // 
            // gbSettings
            // 
            this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSettings.Controls.Add(this.progressBar);
            this.gbSettings.Controls.Add(this.txtStats);
            this.gbSettings.Controls.Add(this.label5);
            this.gbSettings.Controls.Add(this.txtCurrentCell);
            this.gbSettings.Controls.Add(this.label4);
            this.gbSettings.Controls.Add(this.txtCurDefinition);
            this.gbSettings.Controls.Add(this.txtCurEntry);
            this.gbSettings.Controls.Add(this.label2);
            this.gbSettings.Controls.Add(this.label1);
            this.gbSettings.Location = new System.Drawing.Point(702, 392);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(230, 197);
            this.gbSettings.TabIndex = 2;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Statistics";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(9, 172);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(215, 20);
            this.progressBar.TabIndex = 10;
            // 
            // txtStats
            // 
            this.txtStats.Location = new System.Drawing.Point(76, 97);
            this.txtStats.Name = "txtStats";
            this.txtStats.ReadOnly = true;
            this.txtStats.Size = new System.Drawing.Size(148, 20);
            this.txtStats.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Stats:";
            // 
            // txtCurrentCell
            // 
            this.txtCurrentCell.Location = new System.Drawing.Point(76, 71);
            this.txtCurrentCell.Name = "txtCurrentCell";
            this.txtCurrentCell.ReadOnly = true;
            this.txtCurrentCell.Size = new System.Drawing.Size(148, 20);
            this.txtCurrentCell.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Current Cell:";
            // 
            // txtCurDefinition
            // 
            this.txtCurDefinition.Location = new System.Drawing.Point(76, 45);
            this.txtCurDefinition.Name = "txtCurDefinition";
            this.txtCurDefinition.ReadOnly = true;
            this.txtCurDefinition.Size = new System.Drawing.Size(148, 20);
            this.txtCurDefinition.TabIndex = 3;
            // 
            // txtCurEntry
            // 
            this.txtCurEntry.Location = new System.Drawing.Point(76, 19);
            this.txtCurEntry.Name = "txtCurEntry";
            this.txtCurEntry.ReadOnly = true;
            this.txtCurEntry.Size = new System.Drawing.Size(148, 20);
            this.txtCurEntry.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Definition:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current File:";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(944, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFilesToolStripMenuItem,
            this.openFromMPQToolStripMenuItem,
            this.openFromCASCToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadFilesToolStripMenuItem
            // 
            this.loadFilesToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.open;
            this.loadFilesToolStripMenuItem.Name = "loadFilesToolStripMenuItem";
            this.loadFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadFilesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadFilesToolStripMenuItem.Text = "Open File(s)";
            this.loadFilesToolStripMenuItem.Click += new System.EventHandler(this.loadFilesToolStripMenuItem_Click);
            // 
            // openFromMPQToolStripMenuItem
            // 
            this.openFromMPQToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.open;
            this.openFromMPQToolStripMenuItem.Name = "openFromMPQToolStripMenuItem";
            this.openFromMPQToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openFromMPQToolStripMenuItem.Text = "Open from MPQ";
            this.openFromMPQToolStripMenuItem.Click += new System.EventHandler(this.openFromMPQToolStripMenuItem_Click);
            // 
            // openFromCASCToolStripMenuItem
            // 
            this.openFromCASCToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.open;
            this.openFromCASCToolStripMenuItem.Name = "openFromCASCToolStripMenuItem";
            this.openFromCASCToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openFromCASCToolStripMenuItem.Text = "Open from CASC";
            this.openFromCASCToolStripMenuItem.Click += new System.EventHandler(this.openFromCASCToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.save_file;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.save_file;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.reload;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.close;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.close;
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLineToolStripMenuItem,
            this.toolStripSeparator4,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.goToToolStripMenuItem,
            this.toolStripSeparator2,
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.insertToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // newLineToolStripMenuItem
            // 
            this.newLineToolStripMenuItem.Name = "newLineToolStripMenuItem";
            this.newLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newLineToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.newLineToolStripMenuItem.Text = "New Line";
            this.newLineToolStripMenuItem.Click += new System.EventHandler(this.newLineToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(164, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(164, 6);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.goToToolStripMenuItem.Text = "&Go To...";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.gotoIdToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(164, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.findToolStripMenuItem.Text = "&Find...";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.replaceToolStripMenuItem.Text = "&Replace...";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.insertToolStripMenuItem.Text = "Insert Line";
            this.insertToolStripMenuItem.Click += new System.EventHandler(this.insertToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toSQLToolStripMenuItem,
            this.toSQLFileToolStripMenuItem,
            this.toCSVToolStripMenuItem,
            this.toMPQToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toSQLToolStripMenuItem
            // 
            this.toSQLToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.sql;
            this.toSQLToolStripMenuItem.Name = "toSQLToolStripMenuItem";
            this.toSQLToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.toSQLToolStripMenuItem.Text = "To SQL";
            this.toSQLToolStripMenuItem.Click += new System.EventHandler(this.toSQLToolStripMenuItem_Click);
            // 
            // toSQLFileToolStripMenuItem
            // 
            this.toSQLFileToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.sqlfile;
            this.toSQLFileToolStripMenuItem.Name = "toSQLFileToolStripMenuItem";
            this.toSQLFileToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.toSQLFileToolStripMenuItem.Text = "To SQL File";
            this.toSQLFileToolStripMenuItem.Click += new System.EventHandler(this.toSQLFileToolStripMenuItem_Click);
            // 
            // toCSVToolStripMenuItem
            // 
            this.toCSVToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.csv;
            this.toCSVToolStripMenuItem.Name = "toCSVToolStripMenuItem";
            this.toCSVToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.toCSVToolStripMenuItem.Text = "To CSV";
            this.toCSVToolStripMenuItem.Click += new System.EventHandler(this.toCSVToolStripMenuItem_Click);
            // 
            // toMPQToolStripMenuItem
            // 
            this.toMPQToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.sqlfile;
            this.toMPQToolStripMenuItem.Name = "toMPQToolStripMenuItem";
            this.toMPQToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.toMPQToolStripMenuItem.Text = "To MPQ";
            this.toMPQToolStripMenuItem.Click += new System.EventHandler(this.toMPQToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromSQLToolStripMenuItem,
            this.fromCSVToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // fromSQLToolStripMenuItem
            // 
            this.fromSQLToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.sql;
            this.fromSQLToolStripMenuItem.Name = "fromSQLToolStripMenuItem";
            this.fromSQLToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.fromSQLToolStripMenuItem.Text = "From SQL";
            this.fromSQLToolStripMenuItem.Click += new System.EventHandler(this.fromSQLToolStripMenuItem_Click);
            // 
            // fromCSVToolStripMenuItem
            // 
            this.fromCSVToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.csv;
            this.fromCSVToolStripMenuItem.Name = "fromCSVToolStripMenuItem";
            this.fromCSVToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.fromCSVToolStripMenuItem.Text = "From CSV";
            this.fromCSVToolStripMenuItem.Click += new System.EventHandler(this.fromCSVToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editDefinitionsToolStripMenuItem,
            this.wotLKItemFixToolStripMenuItem,
            this.wdb5ParserToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.optionsToolStripMenuItem.Text = "Tools";
            // 
            // editDefinitionsToolStripMenuItem
            // 
            this.editDefinitionsToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.load_definition;
            this.editDefinitionsToolStripMenuItem.Name = "editDefinitionsToolStripMenuItem";
            this.editDefinitionsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editDefinitionsToolStripMenuItem.Text = "Definition Editor";
            this.editDefinitionsToolStripMenuItem.Click += new System.EventHandler(this.editDefinitionsToolStripMenuItem_Click);
            // 
            // wotLKItemFixToolStripMenuItem
            // 
            this.wotLKItemFixToolStripMenuItem.Enabled = false;
            this.wotLKItemFixToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.sql;
            this.wotLKItemFixToolStripMenuItem.Name = "wotLKItemFixToolStripMenuItem";
            this.wotLKItemFixToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.wotLKItemFixToolStripMenuItem.Text = "WotLK Item Import";
            this.wotLKItemFixToolStripMenuItem.Click += new System.EventHandler(this.wotLKItemFixToolStripMenuItem_Click);
            // 
            // wdb5ParserToolStripMenuItem
            // 
            this.wdb5ParserToolStripMenuItem.Image = global::WDBXEditor.Properties.Resources.table;
            this.wdb5ParserToolStripMenuItem.Name = "wdb5ParserToolStripMenuItem";
            this.wdb5ParserToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.wdb5ParserToolStripMenuItem.Text = "WDB5 Parser";
            this.wdb5ParserToolStripMenuItem.Click += new System.EventHandler(this.legionToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "DBC Files|*.dbc|DB2 Files|*.db2|All Files (*.db*)|*.db*";
            this.openFileDialog.Multiselect = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(600, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Columns:";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoIdToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.insertLineToolStripMenuItem,
            this.clearLineToolStripMenuItem,
            this.deleteLineToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(166, 142);
            // 
            // gotoIdToolStripMenuItem
            // 
            this.gotoIdToolStripMenuItem.Name = "gotoIdToolStripMenuItem";
            this.gotoIdToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gotoIdToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.gotoIdToolStripMenuItem.Text = "&Go To...";
            this.gotoIdToolStripMenuItem.Click += new System.EventHandler(this.gotoIdToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.copyToolStripMenuItem.Text = "&Copy Line";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Enabled = false;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.pasteToolStripMenuItem.Text = "&Paste Line";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // insertLineToolStripMenuItem
            // 
            this.insertLineToolStripMenuItem.Name = "insertLineToolStripMenuItem";
            this.insertLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.insertLineToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.insertLineToolStripMenuItem.Text = "Insert Line";
            this.insertLineToolStripMenuItem.Click += new System.EventHandler(this.insertLineToolStripMenuItem_Click);
            // 
            // clearLineToolStripMenuItem
            // 
            this.clearLineToolStripMenuItem.Name = "clearLineToolStripMenuItem";
            this.clearLineToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.clearLineToolStripMenuItem.Text = "Clear Line";
            this.clearLineToolStripMenuItem.Click += new System.EventHandler(this.clearLineToolStripMenuItem_Click);
            // 
            // deleteLineToolStripMenuItem
            // 
            this.deleteLineToolStripMenuItem.Name = "deleteLineToolStripMenuItem";
            this.deleteLineToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.deleteLineToolStripMenuItem.Text = "Delete Line";
            this.deleteLineToolStripMenuItem.Click += new System.EventHandler(this.deleteLineToolStripMenuItem_Click);
            // 
            // columnFilter
            // 
            this.columnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnFilter.ListHeight = 200;
            this.columnFilter.Location = new System.Drawing.Point(656, 3);
            this.columnFilter.Name = "columnFilter";
            this.columnFilter.Size = new System.Drawing.Size(276, 21);
            this.columnFilter.TabIndex = 8;
            this.columnFilter.TabStop = false;
            this.columnFilter.ItemCheckChanged += new System.Windows.Forms.ItemCheckEventHandler(this.columnFilter_ItemCheckChanged);
            this.columnFilter.HideEmptyPressed += new System.EventHandler(this.columnFilter_HideEmptyPressed);
            // 
            // advancedDataGridView
            // 
            this.advancedDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedDataGridView.EnableHeadersVisualStyles = false;
            this.advancedDataGridView.FilterAndSortEnabled = true;
            this.advancedDataGridView.HeaderContext = null;
            this.advancedDataGridView.Location = new System.Drawing.Point(12, 27);
            this.advancedDataGridView.Name = "advancedDataGridView";
            this.advancedDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.advancedDataGridView.RowTemplate.Height = 18;
            this.advancedDataGridView.Size = new System.Drawing.Size(920, 359);
            this.advancedDataGridView.TabIndex = 0;
            this.advancedDataGridView.UndoRedoChanged += new System.EventHandler(this.advancedDataGridView_UndoRedoChanged);
            this.advancedDataGridView.SortStringChanged += new System.EventHandler(this.advancedDataGridView_SortStringChanged);
            this.advancedDataGridView.FilterStringChanged += new System.EventHandler(this.advancedDataGridView_FilterStringChanged);
            this.advancedDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.advancedDataGridView_CellValueChanged);
            this.advancedDataGridView.CurrentCellChanged += new System.EventHandler(this.advancedDataGridView_CurrentCellChanged);
            this.advancedDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.advancedDataGridView_DataBindingComplete);
            this.advancedDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.advancedDataGridView_DefaultValuesNeeded);
            this.advancedDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.advancedDataGridView_RowsAdded);
            this.advancedDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.advancedDataGridView_RowsRemoved);
            this.advancedDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.advancedDataGridView_MouseDown);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 601);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.columnFilter);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbFiles);
            this.Controls.Add(this.advancedDataGridView);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "Main";
            this.Tag = "";
            this.Text = "WDBX Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.gbFiles.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.filecontextMenuStrip.ResumeLayout(false);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AdvancedDataGridView advancedDataGridView;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFilesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private BufferedListBox lbFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurDefinition;
        private System.Windows.Forms.TextBox txtCurEntry;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private DropdownCheckList columnFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toSQLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editDefinitionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoIdToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.TextBox txtCurrentCell;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStats;
        private System.Windows.Forms.ToolStripMenuItem openFromMPQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toMPQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFromCASCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wotLKItemFixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wdb5ParserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private AutoProgressBar progressBar;
        private System.Windows.Forms.ContextMenuStrip filecontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cbBuild;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem deleteLineToolStripMenuItem;
    }
}

