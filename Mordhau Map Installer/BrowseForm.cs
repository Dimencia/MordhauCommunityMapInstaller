using System;
using System.Windows.Forms;

namespace Mordhau_Map_Installer
{
    public partial class BrowseForm : Form
    {
        public BrowseForm()
        {
            InitializeComponent();
            SubmitButton.DialogResult = DialogResult.OK;
            CancelButton.DialogResult = DialogResult.Cancel;
        }

        public string getResultText()
        {
            return textBox1.Text;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}