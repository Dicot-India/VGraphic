using System;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class Options : Form
    {

        public static Main ParentForm { get; set; }
        public int markerInterval {  get; set; }

        public Options()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.change(textBox1.Text, textBox3.Text, textBox4.Text, numericUpDown1.Value, checkBox1.Checked, textBox5.Text, textBox6.Text, checkBox2.Checked);
                Close();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                textBox5.Text = "";
                textBox6.Text = "";
                textBox5.Enabled = false;
                textBox6.Enabled = false;
            }
            else
            {
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = checkBox1.Checked;
        }
    }
}
