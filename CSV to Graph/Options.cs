using System;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class Options : Form
    {

        public static Main ParentForm { get; set; }

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
                ParentForm.change(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, numericUpDown1.Value, numericUpDown2.Value, checkBox1.Checked, textBox5.Text, textBox6.Text);
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
            if(checkBox1.Checked)
            {
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
            }
            else
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
            }
        }
    }
}
