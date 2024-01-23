using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class dateTimeSelector : Form
    {
        public DateTime selectionStart { get; set; } = DateTime.MinValue;
        public DateTime selectionEnd { get; set; } = DateTime.MaxValue;
        public List<string> devices { get; set; } = new List<string>();
        public string selectedFolder { get; set; }

        public dateTimeSelector(List<string> deviceList)
        {
            InitializeComponent();
            this.devices = deviceList;
            startTimePicker.Format = DateTimePickerFormat.Custom;
            EndTimePicker.Format = DateTimePickerFormat.Custom;
            startTimePicker.CustomFormat = "HH:mm:ss dd-MM-yyyy";
            EndTimePicker.CustomFormat = "HH:mm:ss dd-MM-yyyy";
            devicePicker.Items.AddRange(devices.Select(dirPath => Path.GetFileName(dirPath)).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (devicePicker.SelectedIndex > -1)
            {
                selectionStart = startTimePicker.Value;
                selectionEnd = EndTimePicker.Value;
                selectedFolder = devices[devicePicker.SelectedIndex];
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
