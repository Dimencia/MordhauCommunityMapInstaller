using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;

namespace Mordhau_Map_Installer
{
    public partial class Form1 : Form
    {
        private const string MAPS_PATH = @"steamapps\common\mordhau\mordhau\content\mordhau\maps\", VERSION = "1.0.0.21";

        private static readonly string
            s_ApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            s_AppData = $@"{s_ApplicationDataPath}\MordhauMapInstaller",
            s_InfoFiles = $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\InfoFiles\",
            s_InfoFilesZip = $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\InfoFiles.zip";

        private readonly Image m_DefaultThumbnail;
        private string m_MordhauPath = string.Empty;

        public Form1()
        {
            InitializeComponent();
            AvailableMapsBox.SelectedValueChanged += AvailableMapsBoxOnSelectedValueChanged;
            InstalledMapsBox.SelectedValueChanged += AvailableMapsBoxOnSelectedValueChanged;
            MapDescriptionLabel.LinkClicked += MapDescriptionLabelOnLinkClicked;
            selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItemOnClick;
            selectNoneToolStripMenuItem.Click += SelectNoneToolStripMenuItemOnClick;
            selectAllToolStripMenuItem1.Click += SelectAllToolStripMenuItem1OnClick;
            selectNoneToolStripMenuItem1.Click += SelectNoneToolStripMenuItem1OnClick;
            m_DefaultThumbnail = thumbnailBox.Image;

            string version = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4) : VERSION;

            Directory.CreateDirectory($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\");

            Log($"Version {version}");
            versionlabel.Text = $@"Version {version}";

            Directory.CreateDirectory(s_AppData);

            // Remove old log file so we can make a new one
            string logPath = $@"{s_AppData}\log.txt";
            if (File.Exists(logPath))
                File.Delete(logPath);

            // Next check if we have an ini file with mordhau location
            string configPath = $@"{s_AppData}\config.txt";
            if (File.Exists(configPath))
            {
                using (var reader = new StreamReader(configPath))
                    m_MordhauPath = reader.ReadLine();
                Log("Mordhau Path read from configuration file");
            }
            else
            {
                // If no file, show help form
                var help = new HelpForm();
                help.Show(this);
            }
            // Even if we loaded one, make sure it's right, if not loaded this will also be true
            if (!m_MordhauPath.ToLower().Contains(MAPS_PATH) || !Directory.Exists(m_MordhauPath))
            {
                Log("Trying to find Mordhau Directory...");
                string steamPath = GetSteamPath();
                Log($"Steam Path: {steamPath}"); // If steampath contains mordhau, it's the straightup mordhau path
                if (steamPath.Equals(string.Empty) || !Directory.Exists($@"{steamPath}\steamapps\common\mordhau") && !steamPath.ToLower().Contains("mordhau") || !Directory.Exists(steamPath))
                {
                    Log("Mordhau not found");
                    // Open configuration menu to have them browse to it
                    ShowMordhauPathDialog();
                    if (!m_MordhauPath.ToLower().Contains(MAPS_PATH) || !Directory.Exists(m_MordhauPath))
                    {
                        Log($"Invalid Mordhau Path, please re-enter through Settings: {m_MordhauPath}");
                    }
                    Log($"Mordhau Path: {m_MordhauPath}");
                }
                else // If we're here, the directory exists and we have one
                {
                    m_MordhauPath = steamPath.ToLower().Contains("mordhau")
                        ? $@"{steamPath}\mordhau\content\mordhau\maps\"
                        : $@"{steamPath}\steamapps\common\mordhau\mordhau\content\mordhau\maps\";
                    Log($"Mordhau Path: {m_MordhauPath}");
                }
                // Regardless let's save now
                using (var writer = new StreamWriter(configPath))
                {
                    writer.WriteLine(m_MordhauPath);
                }
            }

            // First check for any installed info files and show them
            UpdateInstalledMaps();
            Log("Checking for maps...");
            // Then download the info files from github for available maps and update them
            CheckContents();
        }

        private void SelectNoneToolStripMenuItem1OnClick(object sender, EventArgs e)
        {
            for (var i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, false);
            RemoveButton.Enabled = false;
        }

        private void SelectAllToolStripMenuItem1OnClick(object sender, EventArgs e)
        {
            for (var i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, true);
            RemoveButton.Enabled = true;
        }

        private void SelectNoneToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            for (var i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, false);
            InstallButton.Enabled = false;
        }

        private void SelectAllToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            for (var i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, true);
            InstallButton.Enabled = true;
        }

        private void MapDescriptionLabelOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData is string target && target.ToLower().StartsWith("http"))
                Process.Start(target);
        }

        private void AvailableMapsBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            UpdateMapInfo(((CheckedListBox)sender).SelectedValue as Map);
            InstallButton.Enabled = AvailableMapsBox.CheckedItems.Count > 0;
            RemoveButton.Enabled = InstalledMapsBox.CheckedItems.Count > 0;
            if (m_MordhauPath.ToLower().Contains(@"steamapps\common\mordhau\mordhau\content\mordhau\maps"))
                return;
            RemoveButton.Enabled = false;
            InstallButton.Enabled = false;
            Log("Invalid Mordhau path!  Re-set it in Settings before doing anything");
        }

        private void UpdateMapInfo(Map map)
        {
            if (map == null)
                return;
            MapNameLabel.Text = map.folderName;
            MapDescriptionLabel.Links.Clear();
            MapDescriptionLabel.Text = map.description;

            var linkRegex = new Regex(@"(?i)http(s?):\/\/(\S)*");
            MatchCollection matches = linkRegex.Matches(map.description);
            foreach (Match m in matches)
            {
                int startIndex = m.Index;
                int length = m.Length;
                string link = m.Value;
                MapDescriptionLabel.Links.Add(startIndex, length, link);
            }

            MapAuthorsLabel.Text = map.authors;
            MapVersionLabel.Text = map.version;
            MapReleaseDateLabel.Text = map.releaseDate.ToLongDateString();
            MapSizeLabel.Text = map.fileSize;
            MapSuggestedPlayersLabel.Text = map.suggestedPlayers;
            if (string.IsNullOrEmpty(map.thumbnailURL) || !map.thumbnailURL.ToLower().StartsWith("http"))
            {
                thumbnailBox.Image = m_DefaultThumbnail;
            }
            else
            {
                thumbnailBox.ImageLocation = map.thumbnailURL;
            }
        }

        private async Task CheckContents()
        {
            try
            {
                Map.maps = new List<Map>();

                Log("Getting all contents");

                Log("Deleting old zip");
                // Delete zip if it already exists, though it shouldn't. 
                if (File.Exists(s_InfoFilesZip))
                    File.Delete(s_InfoFilesZip);

                Log("Downloading file");
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(
                        @"https://github.com/MordhauMappingModding/InfoFiles/archive/master.zip",
                        s_InfoFilesZip);
                }

                Log("Clearing existing info files");
                // Clear existing info files to prevent conflicts or old files getting stuck on file
                if (Directory.Exists(s_InfoFiles))
                    Directory.Delete(s_InfoFiles, true);

                Log("Creating info file directory");

                Directory.CreateDirectory(s_InfoFiles);

                Log("Extracting Info Files");

                ZipFile.ExtractToDirectory(s_InfoFilesZip,
                    s_InfoFiles);

                Log("Deleting Info Zip");

                File.Delete(s_InfoFilesZip);

                Log("Checking resulting files");
                foreach (string s in Directory.GetFiles(s_InfoFiles + @"InfoFiles-master\"))
                {
                    var m = new Map();
                    // Open the file, get it into a string, and split it on newlines
                    try
                    {
                        using (var reader = new StreamReader(s))
                        {
                            string content = reader.ReadToEnd().Replace("\r\n", "\n"); // So we can split on it
                            string[]
                                lines = content.Split('\n'); // Could have done this with ReadLine but already had this
                            m.name = lines[MapVars.name];
                            m.folderName = lines[MapVars.folderName];
                            m.description = lines[MapVars.description];
                            m.authors = lines[MapVars.authors];
                            m.version = lines[MapVars.version];
                            m.releaseDate = DateTime.ParseExact(lines[MapVars.releaseDate], "dd/MM/yyyy",
                                CultureInfo.InvariantCulture);
                            m.fileSize = lines[MapVars.fileSize];
                            if (lines.Length > MapVars.suggestedPlayers)
                                m.suggestedPlayers = lines[MapVars.suggestedPlayers];
                            if (lines.Length > MapVars.thumbnailURL)
                                m.thumbnailURL = lines[MapVars.thumbnailURL];
                            Map.maps.Add(m);
                            foreach (Map y in Map.installed.Where(z =>
                                z.folderName.Equals(m.folderName) && !z.version.Equals(m.version) && !z.needsUpdate))
                            {
                                y.needsUpdate = true;
                                y.name = $"[OUTDATED] {y.name}";
                                InstalledMapsBox.Refresh();
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log($"Error in info file: {s}; {e.Message}");
                    }

                }

                Log("Updating available maps");
                UpdateAvailableMaps();
                Update();
                Log("Ready");
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log("Error while updating maps.  File -> Refresh to try again");
            }
        }

        private void UpdateAvailableMaps()
        {
            try
            {
                Log($"{Map.maps.Count} maps being updated");
                if(AvailableMapsBox.DataSource == null)
                    AvailableMapsBox.DataSource = Map.maps;
                AvailableMapsBox.DisplayMember = "name";
                Log($"{Map.maps.Count} maps updated");
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log("Refresh Failed");
            }
        }

        private void ShowMordhauPathDialog()
        {
            var browseForm = new BrowseForm();
            DialogResult result = browseForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!browseForm.getResultText().ToLower().Contains("mordhau") || !Directory.Exists(browseForm.getResultText()))
                {
                    m_MordhauPath = string.Empty;
                    Log("Invalid Mordhau Path!  Re-set through Settings");
                }
                else
                {
                    m_MordhauPath = $@"{browseForm.getResultText()}\mordhau\content\mordhau\maps\";
                    Log($"New Mordhau Path: {m_MordhauPath}");
                }
            }
            browseForm.Dispose();
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
            labelspacer.Text = message;
            using (StreamWriter writer = File.AppendText($@"{s_ApplicationDataPath}\MordhauMapInstaller\log.txt"))
            {
                writer.WriteLine(message);
            }
        }

        private string GetSteamPath()
        {
            // This is now a 64-bit program so has no problems getting into the registry
            var steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 629760", "InstallLocation", string.Empty);
            if (steamPath != null)
                Log("Successfully read from Mordhau InstallLocation");
            if (string.IsNullOrEmpty(steamPath))
                steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", string.Empty);
            if (string.IsNullOrEmpty(steamPath))
                steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath",
                    steamPath);

            return steamPath ?? string.Empty;
        }

        private void setMordhauDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMordhauPathDialog();
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            // Download and unzip each map into MordhauPath
            // Then delete the zip to keep it clean
            
            InstallButton.Enabled = false;
            foreach (Map m in AvailableMapsBox.CheckedItems)
            {
                Log("Beginning install for " + m.name);
                InstallMap(m);
            }

            for (var i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, false);

            UpdateInstalledMaps();
        }

        private void UpdateInstalledMaps()
        {
            Map.installed = new List<Map>();
            // Find all files ending with .info.txt in mordhaupath
            foreach (string f in Directory.GetFiles(m_MordhauPath, "*.info.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    var m = new Map();
                    try
                    {
                        string content = reader.ReadToEnd().Replace("\r\n", "\n");
                        string[] lines = content.Split('\n');
                        m.name = lines[MapVars.name];
                        m.folderName = lines[MapVars.folderName];
                        m.description = lines[MapVars.description];
                        m.authors = lines[MapVars.authors];
                        m.version = lines[MapVars.version];
                        m.releaseDate = DateTime.ParseExact(lines[MapVars.releaseDate], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        m.fileSize = lines[MapVars.fileSize];
                        if (lines.Length > MapVars.suggestedPlayers)
                            m.suggestedPlayers = lines[MapVars.suggestedPlayers];
                        if (lines.Length > MapVars.thumbnailURL)
                            m.thumbnailURL = lines[MapVars.thumbnailURL];
                        //Log("Adding map " + m.name);
                        Map.installed.Add(m);
                    }
                    catch (Exception e)
                    {
                        Log($"Error in installed info file: {f}; {e.Message}");
                    }
                }
            }
            foreach (Map m in Map.maps)
            {
                foreach (Map y in Map.installed.Where(z =>
                    z.folderName.Equals(m.folderName) && !z.version.Equals(m.version) && !z.needsUpdate))
                {
                    y.needsUpdate = true;
                    y.name = $"[OUTDATED] {y.name}";
                }
            }
            InstalledMapsBox.DataSource = Map.installed;
            InstalledMapsBox.DisplayMember = "name";
            Update();
        }

        private async Task InstallMap(Map m)
        {
            try
            {
                Log($@"Downloading {m.name} as \{m.folderName}.zip");
                // https://github.com/MordhauMappingModding/MapFiles/blob/master/BerzerkerArena.zip
                // This fucking api can't handle downloading zip files so I have to do it by hand and hope we don't ratelimit
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(
                        $@"https://github.com/MordhauMappingModding/MapFiles/blob/master/{m.folderName}.zip?raw=true",
                        $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{m.folderName}.zip");
                }

                // Check if folder already exists and delete if it does
                string mapFolder = $"{m_MordhauPath}{m.folderName}";
                if (Directory.Exists(mapFolder))
                    Directory.Delete(mapFolder, true);
                Log($"Extracting {m.name}");
                ZipFile.ExtractToDirectory($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{m.folderName}.zip",
                    m_MordhauPath);
                Log($"Deleting {m.name}");
                File.Delete($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{m.folderName}.zip");
                // Copy over the info file
                File.Copy($@"{s_InfoFiles}\InfoFiles-master\{m.folderName}.info.txt",
                    $"{m_MordhauPath}{m.folderName}.info.txt", true);
                Log($"Successfully installed {m.name}");
            }
            catch (Exception e)
            {
                Log($"Failed during installation: {e.Message}");
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (Map m in InstalledMapsBox.CheckedItems)
            {
                try
                {
                    Directory.Delete($"{m_MordhauPath}{m.folderName}", true);
                    Log($"Successfully deleted {m.name}");
                }
                catch (Exception ex)
                {
                    Log($"Error while deleting {m.name}: {ex.Message}\n{ex.StackTrace}");
                }
                try
                {
                    File.Delete($"{m_MordhauPath}{m.folderName}.info.txt");
                }
                catch (Exception ex)
                {
                    Log($"Error while deleting {m.name}: {ex.Message}\n{ex.StackTrace}");
                }
            }
            for (var i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, false);
            RemoveButton.Enabled = false;
            UpdateInstalledMaps();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateInstalledMaps();
                Log("Refreshing...");
                CheckContents();
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new AboutForm();
            f.ShowDialog();
        }

        private void basicHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var help = new HelpForm();
            help.Show();
        }
    }

    public static class MapVars
    {
        public static int name = 0;
        public static int folderName = 1;
        public static int description = 2;
        public static int authors = 3;
        public static int version = 4;
        public static int releaseDate = 5;
        public static int fileSize = 6;
        public static int suggestedPlayers = 7;
        public static int thumbnailURL = 8;
    }
}