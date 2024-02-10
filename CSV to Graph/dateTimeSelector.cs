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
        public string selectedFile { get; set; }
        public FileDataList selectedInfo { get; set; }

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
            if (devicePicker.SelectedIndex > -1 && fileListSelector.SelectedIndex > -1)
            {
                selectionStart = startTimePicker.Value;
                selectionEnd = EndTimePicker.Value;
                selectedFolder = devices[devicePicker.SelectedIndex];
                selectedFile = selectedInfo.fileList[fileListSelector.SelectedIndex].fileName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void devicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string path = devices[devicePicker.SelectedIndex];
                FileDataList fileInformation = new FileDataList(path);
                selectedInfo = fileInformation;
                fileListSelector.Items.Clear();

                if (fileInformation != null && fileInformation.fileList.Count > 0)
                {
                    fileInfo startFile = fileInformation.fileList[0];
                    DateTime fileStartTime = new DateTime(startFile.startTimeStamp);
                    DateTime fileEndTime = new DateTime(startFile.endTimeStamp);

                    foreach (fileInfo info in  fileInformation.fileList) 
                    { 
                        fileStartTime = fileStartTime > new DateTime(info.startTimeStamp) ? new DateTime(info.startTimeStamp) : fileStartTime;
                        fileEndTime = fileEndTime < new DateTime(info.endTimeStamp) ? new DateTime(info.endTimeStamp) : fileEndTime;
                    }

                    startTimePicker.MaxDate = fileEndTime;
                    startTimePicker.MinDate = fileStartTime;
                    startTimePicker.Value = fileStartTime;

                    EndTimePicker.MaxDate = fileEndTime;
                    EndTimePicker.MinDate = fileStartTime;
                    EndTimePicker.Value = fileEndTime;

                    List<string> fileList = fileInformation.fileList.Select((file) => file.fileName).ToList();
                    fileListSelector.Items.AddRange(fileList.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dateTimeSelector_Load(object sender, EventArgs e)
        {

        }

        private void fileListSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fileInfo currentFile = selectedInfo.fileList.FirstOrDefault(file => file.fileName == selectedInfo.fileList[fileListSelector.SelectedIndex].fileName);
                DateTime fileStartTime = new DateTime(currentFile.startTimeStamp);
                DateTime fileEndTime = new DateTime(currentFile.endTimeStamp);
                startTimePicker.Value = fileStartTime;

                EndTimePicker.Value = fileEndTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
