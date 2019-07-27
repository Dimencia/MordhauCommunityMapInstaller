using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string target = e.Link.LinkData as string;
            if (target != null && target.ToLower().StartsWith("http"))
                System.Diagnostics.Process.Start(target);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
