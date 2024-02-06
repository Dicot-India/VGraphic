using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class chartSetup : Form
    {
        string imagePath = "";
        public chartSetup()
        {
            InitializeComponent();
            imagePath = Properties.Settings.Default.chartLogo;
            if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
            }
            checkBox1.Checked = Properties.Settings.Default.chartStat;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            imagePath = string.Empty;
            pictureBox1.Image = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = fileDialog.FileName;
                if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.chartStat = checkBox1.Checked;
            Properties.Settings.Default.chartLogo = imagePath;
            Properties.Settings.Default.Save();
            MessageBox.Show("The changes were saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
