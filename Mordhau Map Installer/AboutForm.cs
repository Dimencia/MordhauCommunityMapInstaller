using System.Diagnostics;
using System.Windows.Forms;

namespace Mordhau_Map_Installer
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            linkLabel1.Links.Clear();
            linkLabel1.Links.Add(4, 26, "https://discord.gg/68TGxv4");
            linkLabel1.LinkClicked += LinkLabel1OnLinkClicked;
        }

        private void LinkLabel1OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData is string target && target.ToLower().StartsWith("http"))
                Process.Start(target);
        }
    }
}