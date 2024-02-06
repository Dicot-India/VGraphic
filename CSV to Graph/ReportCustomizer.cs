using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class ReportCustomizer : Form
    {
        string imagePath { get; set; }

        public ReportCustomizer()
        {
            InitializeComponent();
            titleText.Text = Properties.Settings.Default.Title;
            subTitleText.Text = Properties.Settings.Default.Subtitle;
            imagePath = Properties.Settings.Default.Logo;
            if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
            }
            checkBox1.Checked = Properties.Settings.Default.tableStat;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Title = titleText.Text;
            Properties.Settings.Default.Subtitle = subTitleText.Text;
            Properties.Settings.Default.Logo = imagePath;
            Properties.Settings.Default.tableStat = checkBox1.Checked;
            Properties.Settings.Default.Save();
            MessageBox.Show("The changes were saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imagePath = string.Empty; 
            pictureBox1.Image = null;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = fileDialog.FileName;
                if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath);
                }
            }
        }
    }
}
