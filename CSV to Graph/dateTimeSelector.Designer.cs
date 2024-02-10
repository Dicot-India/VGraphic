namespace CSV_Graph
{
    partial class dateTimeSelector
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
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.devicePicker = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileListSelector = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // startTimePicker
            // 
            this.startTimePicker.Location = new System.Drawing.Point(16, 87);
            this.startTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new System.Drawing.Size(229, 22);
            this.startTimePicker.TabIndex = 0;
            // 
            // EndTimePicker
            // 
            this.EndTimePicker.Location = new System.Drawing.Point(16, 130);
            this.EndTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EndTimePicker.Name = "EndTimePicker";
            this.EndTimePicker.Size = new System.Drawing.Size(229, 22);
            this.EndTimePicker.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "To";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(141, 177);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 37);
            this.button1.TabIndex = 3;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(16, 177);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 37);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Device ID:";
            // 
            // devicePicker
            // 
            this.devicePicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicePicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.devicePicker.FormattingEnabled = true;
            this.devicePicker.Location = new System.Drawing.Point(92, 14);
            this.devicePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.devicePicker.Name = "devicePicker";
            this.devicePicker.Size = new System.Drawing.Size(89, 24);
            this.devicePicker.TabIndex = 5;
            this.devicePicker.SelectedIndexChanged += new System.EventHandler(this.devicePicker_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "File:";
            // 
            // fileListSelector
            // 
            this.fileListSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileListSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fileListSelector.FormattingEnabled = true;
            this.fileListSelector.Location = new System.Drawing.Point(84, 51);
            this.fileListSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fileListSelector.Name = "fileListSelector";
            this.fileListSelector.Size = new System.Drawing.Size(161, 24);
            this.fileListSelector.TabIndex = 7;
            this.fileListSelector.SelectedIndexChanged += new System.EventHandler(this.fileListSelector_SelectedIndexChanged);
            // 
            // dateTimeSelector
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(291, 259);
            this.ControlBox = false;
            this.Controls.Add(this.fileListSelector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.devicePicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EndTimePicker);
            this.Controls.Add(this.startTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dateTimeSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Data";
            this.Load += new System.EventHandler(this.dateTimeSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.DateTimePicker EndTimePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox devicePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox fileListSelector;
    }
}