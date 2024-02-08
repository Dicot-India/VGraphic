namespace CSV_Graph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAspngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.clearFilter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.exportPDFButton = new System.Windows.Forms.Button();
            this.filterButton = new System.Windows.Forms.Button();
            this.filterEnd = new System.Windows.Forms.DateTimePicker();
            this.filterStart = new System.Windows.Forms.DateTimePicker();
            this.dataView = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Average = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Menu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.BackColor = System.Drawing.SystemColors.Control;
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.Menu.Size = new System.Drawing.Size(1348, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.databaseToolStripMenuItem.Text = "Database";
            this.databaseToolStripMenuItem.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAspngToolStripMenuItem,
            this.printToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.helpToolStripMenuItem.Enabled = false;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.helpToolStripMenuItem.Text = "Graph";
            // 
            // saveAspngToolStripMenuItem
            // 
            this.saveAspngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pngToolStripMenuItem,
            this.pdfToolStripMenuItem});
            this.saveAspngToolStripMenuItem.Name = "saveAspngToolStripMenuItem";
            this.saveAspngToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAspngToolStripMenuItem.Text = "Save As";
            // 
            // pngToolStripMenuItem
            // 
            this.pngToolStripMenuItem.Name = "pngToolStripMenuItem";
            this.pngToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.pngToolStripMenuItem.Text = ".png";
            this.pngToolStripMenuItem.Click += new System.EventHandler(this.pngToolStripMenuItem_Click);
            // 
            // pdfToolStripMenuItem
            // 
            this.pdfToolStripMenuItem.Name = "pdfToolStripMenuItem";
            this.pdfToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.pdfToolStripMenuItem.Text = ".pdf";
            this.pdfToolStripMenuItem.Click += new System.EventHandler(this.pdfToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.printToolStripMenuItem.Text = "PDF Setup";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // OFD
            // 
            this.OFD.Filter = "DAT (*.dat)|*.dat";
            this.OFD.FileOk += new System.ComponentModel.CancelEventHandler(this.OFD_FileOk);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1348, 822);
            this.tabControl.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1340, 793);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Graph";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1340, 793);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 2);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.linkLabel1);
            this.splitContainer.Panel1.Controls.Add(this.button2);
            this.splitContainer.Panel1.Controls.Add(this.button1);
            this.splitContainer.Panel1.Controls.Add(this.clearFilter);
            this.splitContainer.Panel1.Controls.Add(this.label2);
            this.splitContainer.Panel1.Controls.Add(this.exportPDFButton);
            this.splitContainer.Panel1.Controls.Add(this.filterButton);
            this.splitContainer.Panel1.Controls.Add(this.filterEnd);
            this.splitContainer.Panel1.Controls.Add(this.filterStart);
            this.splitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataView);
            this.splitContainer.Size = new System.Drawing.Size(1334, 789);
            this.splitContainer.SplitterDistance = 67;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 35);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(116, 17);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Filter Parameters";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1208, 6);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "PDF Setup";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(782, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 36);
            this.button1.TabIndex = 6;
            this.button1.Text = "Export CSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clearFilter
            // 
            this.clearFilter.Location = new System.Drawing.Point(655, 7);
            this.clearFilter.Margin = new System.Windows.Forms.Padding(4);
            this.clearFilter.Name = "clearFilter";
            this.clearFilter.Size = new System.Drawing.Size(119, 36);
            this.clearFilter.TabIndex = 5;
            this.clearFilter.Text = "Clear Filter";
            this.clearFilter.UseVisualStyleBackColor = true;
            this.clearFilter.Click += new System.EventHandler(this.clearFilter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "to";
            // 
            // exportPDFButton
            // 
            this.exportPDFButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPDFButton.Location = new System.Drawing.Point(1077, 6);
            this.exportPDFButton.Margin = new System.Windows.Forms.Padding(4);
            this.exportPDFButton.Name = "exportPDFButton";
            this.exportPDFButton.Size = new System.Drawing.Size(123, 37);
            this.exportPDFButton.TabIndex = 3;
            this.exportPDFButton.Text = "Export PDF";
            this.exportPDFButton.UseVisualStyleBackColor = true;
            this.exportPDFButton.Click += new System.EventHandler(this.exportPDFButton_Click);
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(528, 7);
            this.filterButton.Margin = new System.Windows.Forms.Padding(4);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(119, 36);
            this.filterButton.TabIndex = 2;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // filterEnd
            // 
            this.filterEnd.CustomFormat = "hh:mm:ss tt dd/MM/yyyy";
            this.filterEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filterEnd.Location = new System.Drawing.Point(273, 9);
            this.filterEnd.Margin = new System.Windows.Forms.Padding(4);
            this.filterEnd.Name = "filterEnd";
            this.filterEnd.Size = new System.Drawing.Size(232, 23);
            this.filterEnd.TabIndex = 1;
            // 
            // filterStart
            // 
            this.filterStart.CustomFormat = "hh:mm:ss tt dd/MM/yyyy";
            this.filterStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filterStart.Location = new System.Drawing.Point(6, 9);
            this.filterStart.Margin = new System.Windows.Forms.Padding(4);
            this.filterStart.Name = "filterStart";
            this.filterStart.Size = new System.Drawing.Size(232, 23);
            this.filterStart.TabIndex = 0;
            // 
            // dataView
            // 
            this.dataView.AllowUserToAddRows = false;
            this.dataView.AllowUserToDeleteRows = false;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(0, 0);
            this.dataView.Margin = new System.Windows.Forms.Padding(4);
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.RowHeadersWidth = 51;
            this.dataView.Size = new System.Drawing.Size(1334, 717);
            this.dataView.TabIndex = 0;
            this.dataView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellContentClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1340, 793);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Statistics";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelName,
            this.Minimum,
            this.Maximum,
            this.Average});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1334, 787);
            this.dataGridView1.TabIndex = 0;
            // 
            // ChannelName
            // 
            this.ChannelName.HeaderText = "Channel Name";
            this.ChannelName.Name = "ChannelName";
            this.ChannelName.ReadOnly = true;
            // 
            // Minimum
            // 
            this.Minimum.HeaderText = "Minimum Value";
            this.Minimum.Name = "Minimum";
            this.Minimum.ReadOnly = true;
            // 
            // Maximum
            // 
            this.Maximum.HeaderText = "Maximum Value";
            this.Maximum.Name = "Maximum";
            this.Maximum.ReadOnly = true;
            // 
            // Average
            // 
            this.Average.HeaderText = "Average Value";
            this.Average.Name = "Average";
            this.Average.ReadOnly = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 846);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.Menu);
            this.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.Menu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sansel Soft 01";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.ToolStripMenuItem saveAspngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SFD;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dataView;
        private System.Windows.Forms.DateTimePicker filterEnd;
        private System.Windows.Forms.DateTimePicker filterStart;
        private System.Windows.Forms.Button exportPDFButton;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clearFilter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Maximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Average;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}