using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mordhau_Map_Installer
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            discordlink.Links.Clear();
            discordlink.Links.Add(0, 34, "https://discord.gg/68TGxv4");
            discordlink.LinkClicked += DiscordlinkOnLinkClicked;
        }

        private void DiscordlinkOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData is string target && target.ToLower().StartsWith("http"))
                Process.Start(target);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}