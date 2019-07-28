using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Mordhau_Map_Installer
{
    public partial class UpdateAvailable : Form
    {
        private bool disableUpdates = false;

        public UpdateAvailable(string detectedVersion, string changeSummary)
        {
            InitializeComponent();
            currentVersionLabel.Text = Form1.VERSION;
            newVersionLabel.Text = detectedVersion;
            summaryLabel.Text = changeSummary;
            dontShowAgainBox.Checked = !Form1.checkForUpdates;
            githubLink.Links.Clear();
            githubLink.Links.Add(6, 4, Form1.s_ProjectGithub);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Easier than adjusting the config file
                if (disableUpdates)
                {
                    if(!File.Exists(Form1.s_DisableUpdatesFile))
                        File.Create(Form1.s_DisableUpdatesFile);
                }
                else
                {
                    if(File.Exists(Form1.s_DisableUpdatesFile))
                        File.Delete(Form1.s_DisableUpdatesFile);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
                Log("Failed to save auto-update settings to config file");
            }
            
            Close();
        }

        private void Log(string message)
        {
            using (StreamWriter writer = File.AppendText($@"{Form1.s_ApplicationDataPath}\MordhauMapInstaller\log.txt"))
            {
                writer.WriteLine(message);
            }
        }

        private void dontShowAgainBox_CheckedChanged(object sender, EventArgs e)
        {
            disableUpdates = dontShowAgainBox.Checked;
        }
    }
}
