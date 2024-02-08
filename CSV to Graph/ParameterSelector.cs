using System.Collections.Generic;
using System.Windows.Forms;

namespace CSV_Graph
{
    public partial class ParameterSelctor : Form
    {
        List<string> channelNames = new List<string>();
        public List<int> selectedChannelID { get; set; } = new List<int>();

        public ParameterSelctor(List<string> channelNames)
        {
            InitializeComponent();
            this.channelNames = channelNames;
            if(channelNames.Count > 0 )
            {
                foreach (var item in channelNames)
                {
                    checkedListBox1.Items.Add(item);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox1.Checked);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            foreach (int index in checkedListBox1.CheckedIndices)
            {
                selectedChannelID.Add(index);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
