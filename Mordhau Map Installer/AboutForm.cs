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
            string target = e.Link.LinkData as string;
            if (target != null && target.ToLower().StartsWith("http"))
                System.Diagnostics.Process.Start(target);
        }
    }
}
