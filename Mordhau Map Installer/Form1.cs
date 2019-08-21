using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;

namespace Mordhau_Map_Installer
{
    public partial class Form1 : Form
    {
        public const string MAPS_PATH = @"steamapps\common\mordhau\mordhau\content\mordhau\maps\", VERSION = "1.1.0.4";

        public static readonly string
            s_ApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            s_AppData = $@"{s_ApplicationDataPath}\MordhauMapInstaller",
            s_InfoFiles = $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\InfoFiles\",
            s_InfoFilesZip = $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\InfoFiles.zip",
            s_ProjectGithub = @"https://github.com/Dimencia/MordhauCommunityMapInstaller",
            s_DisableUpdatesFile = $@"{s_AppData}\noupdates.txt",
            configPath = $@"{s_AppData}\config.txt",
            s_InfoFilesURL = @"https://github.com/MordhauMappingModding/InfoFiles/archive/master.zip",
            s_InfoFilesFolder = @"InfoFiles-master\";
        // InfoFiles-master
        public static int maxSimultaneousMapDownloads = 10;

        private readonly Image m_DefaultThumbnail;
        private string m_MordhauPath = string.Empty;
        public static bool checkForUpdates = true;

        //private int progressWidth = 696;
        //private int progressHeight = 40;

        public Form1()
        {
            //System.Globalization.CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
            InitializeComponent();
            AvailableMapsBox.SelectedValueChanged += AvailableMapsBoxOnSelectedValueChanged;
            InstalledMapsBox.SelectedValueChanged += AvailableMapsBoxOnSelectedValueChanged;
            MapDescriptionLabel.LinkClicked += MapDescriptionLabelOnLinkClicked;
            selectAllToolStripMenuItem.Click += SelectAllToolStripMenuItemOnClick;
            selectNoneToolStripMenuItem.Click += SelectNoneToolStripMenuItemOnClick;
            selectAllToolStripMenuItem1.Click += SelectAllToolStripMenuItem1OnClick;
            selectNoneToolStripMenuItem1.Click += SelectNoneToolStripMenuItem1OnClick;
            m_DefaultThumbnail = thumbnailBox.Image;
            //696, 40
            //progressWidth = progressBar1.Size.Width;
            //progressHeight = progressBar1.Size.Height;
            //progressBar1.Size = new Size(0,0);

            string version = ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4) : VERSION;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;



            Directory.CreateDirectory($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\");

            Log($"Version {version}");
            versionlabel.Text = $@"Version {version}";

            Directory.CreateDirectory(s_AppData);

            // Remove old log file so we can make a new one
            string logPath = $@"{s_AppData}\log.txt";
            if (File.Exists(logPath))
                File.Delete(logPath);

            checkForUpdates = !File.Exists(s_DisableUpdatesFile);

            // Check for updates on startup
            CheckForUpdates();

            // Next check if we have an ini file with mordhau location

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
                // If steamPath doesn't contain "Mordhau", it is the actual steam path
                // So then also if that path + \steamapps\common\mordhau\mordhau.exe doesn't exist, its invalid
                // If its empty, its invalid
                // If the directory does not exist, its invalid
                // We should never get a directory containing 'Mordhau' that hasn't already been checked for validity
                if (steamPath.Equals(string.Empty) || (!File.Exists($@"{steamPath}\steamapps\common\mordhau\Mordhau.exe") && !steamPath.ToLower().Contains("mordhau")) || !Directory.Exists(steamPath))
                {
                    Log("Mordhau not found");
                    // Open configuration menu to have them browse to it
                    ShowMordhauPathDialog();
                    if (!m_MordhauPath.ToLower().Contains(MAPS_PATH) || !Directory.Exists(m_MordhauPath))
                    {
                        Log($"Mordhau Path: {m_MordhauPath}");
                        Log(Properties.Resources.str_Invalid_Mordhau_Path_please_re_enter);
                    }
                }
                else // If we're here, the directory exists and we have one
                {
                    m_MordhauPath = steamPath.ToLower().Contains("mordhau")
                        ? $@"{steamPath}\mordhau\content\mordhau\maps\"
                        : $@"{steamPath}\steamapps\common\mordhau\mordhau\content\mordhau\maps\";
                    Log($"Mordhau Path: {m_MordhauPath}");
                    UpdateEverything();
                }
            }
            else
            {
                UpdateEverything();
            }
        }

        private void UpdateEverything()
        {
            // Regardless let's save now... why am I doing it like this, no idea
            using (var writer = new StreamWriter(configPath))
            {
                writer.WriteLine(m_MordhauPath);
            }
            // Check for any installed info files and show them
            UpdateInstalledMaps();
            Log("Checking for maps...");
            // Then download the info files from github for available maps and update them
            CheckContents();
            Log("Updating available maps");
            UpdateAvailableMaps();
            CheckMapUpdates();
            Update();
            Log(Properties.Resources.str_Ready);
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
            Log(Properties.Resources.str_Invalid_Mordhau_path_Re_set_it_in);
        }

        private void CheckForUpdates(bool manual = false)
        {
            try
            {
                // Load the webpage
                using (var client = new WebClient())
                {
                    // Load the updateInfo file
                    // https://github.com/Dimencia/MordhauCommunityMapInstaller/raw/master/UpdateInfo.txt
                    string contents = client.DownloadString("https://github.com/Dimencia/MordhauCommunityMapInstaller/raw/master/UpdateInfo.txt");
                    // Normalize line endings
                    contents = contents.Replace("\r\n", "\n");
                    var lines = contents.Split('\n');
                    string version = lines[0];
                    string url = lines[1];
                    string filename = lines[2];
                    bool required = bool.Parse(lines[3]);
                    string changelog = lines[4];

                    Log($"Detected version {version} vs current {VERSION}");
                    Log(changelog);
                    if ((checkForUpdates || required || manual) && !version.Equals(VERSION))
                    {
                        UpdateAvailable updateForm = new UpdateAvailable(version, url, filename, changelog);
                        updateForm.Show(this);
                        return;
                    }
                    else
                        Log(Properties.Resources.str_No_updates_found);
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log(Properties.Resources.str_Failed_to_check_for_updates);
            }
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
                    wc.DownloadFile(s_InfoFilesURL,
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
                foreach (string s in Directory.GetFiles(s_InfoFiles + s_InfoFilesFolder))
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
                            if (lines.Length > MapVars.downloadURL && lines[MapVars.downloadURL].Length > 0)
                                m.downloadURL = GetDownloadLink(lines[MapVars.downloadURL]);
                            else
                                m.downloadURL =
                                    $@"https://github.com/MordhauMappingModding/MapFiles/blob/master/{m.folderName}.zip?raw=true";
                            Map.maps.Add(m);
                        }

                    }
                    catch (Exception e)
                    {
                        Log($"Error in info file: {s}; {e.Message}");
                    }

                }
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log(Properties.Resources.str_Error_while_updating_maps_File);
            }
        }

        private void UpdateAvailableMaps()
        {
            try
            {
                Log($"{Map.maps.Count} maps being updated");
                if (AvailableMapsBox.DataSource == null)
                    AvailableMapsBox.DataSource = Map.maps;
                AvailableMapsBox.DisplayMember = "name";
                Log($"{Map.maps.Count} maps updated");
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log(Properties.Resources.str_Refresh_Failed);
            }
        }

        private string GetDownloadLink(string link)
        {
            // This parses a link and returns a direct download link
            // If given a link other than GDrive, returns the same link
            // Otherwise parses a gdrive link and returns an API key'd direct download

            //Styles to expect:
            //https://drive.google.com/open?id=1iUzGkUOXhaoxXy7thSd11cQs4I06aLXP
            //https://drive.google.com/file/d/1iUzGkUOXhaoxXy7thSd11cQs4I06aLXP/view
            //https://drive.google.com/uc?id=1iUzGkUOXhaoxXy7thSd11cQs4I06aLXP&export=download

            if (!link.ToLower().Contains("https://drive.google.com/"))
                return link;

            string result = string.Empty;

            if (link.Contains("?id="))
            {
                Regex reg = new Regex(@"\?id=([^&]*)");
                var match = reg.Match(link);
                if (match.Success)
                {
                    result = match.Groups[1].Value;
                }
                else
                {
                    Log($"Error parsing GDrive link {link}");
                    return string.Empty;
                }
            }
            else
            {
                Regex reg = new Regex(@"\/file\/d\/([^\/]*)");
                var match = reg.Match(link);
                if (match.Success)
                {
                    result = match.Groups[1].Value;
                }
                else
                {
                    Log($"Error parsing GDrive link {link}");
                    return string.Empty;
                }
            }

            return
                $@"https://www.googleapis.com/drive/v3/files/{result}/?key={Properties.Resources.google_api_key}&alt=media";
        }

        private void ShowMordhauPathDialog()
        {
            var browseForm = new BrowseForm(m_MordhauPath);
            DialogResult result = browseForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!browseForm.getResultText().ToLower().Contains("mordhau") || !Directory.Exists(browseForm.getResultText()) || !File.Exists($@"{browseForm.getResultText()}\Mordhau.exe"))
                {
                    m_MordhauPath = string.Empty;
                    Log(Properties.Resources.str_Invalid_Mordhau_Path_Re_set_through);
                }
                else
                {
                    m_MordhauPath = $@"{browseForm.getResultText()}\mordhau\content\mordhau\maps\";
                    Log($"New Mordhau Path: {m_MordhauPath}");
                    UpdateEverything();
                }
            }
            browseForm.Dispose();

        }

        private delegate void SafeCallDelegate(string text);

        private static object locker = new Object();

        public void Log(string message)
        {
            Console.WriteLine(message);
            if (InvokeRequired)
            {
                var d = new SafeCallDelegate(Log);
                Invoke(d, new object[] { message });
            }
            else
            {
                labelspacer.Text = message;
                try
                {
                    lock (locker)
                        using (StreamWriter writer =
                            File.AppendText($@"{s_ApplicationDataPath}\MordhauMapInstaller\log.txt"))
                        {
                            writer.WriteLine(message);
                        }
                }
                catch (Exception e)
                {
                    Log(e.Message);
                    Log(e.StackTrace);
                    Log($"Failed to write log: {message}");
                }
            }
        }

        private string GetSteamPath()
        {
            // This is now a 64-bit program so has no problems getting into the registry
            var steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 629760", "InstallLocation", string.Empty);
            if (steamPath != null)
            {
                if (!File.Exists($@"{steamPath}\Mordhau.exe"))
                {
                    Log("InstallLocation found, but was invalid");
                    steamPath = null;
                }
                else
                    Log("Successfully read from Mordhau InstallLocation");
            }

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

        private delegate void SafeVoid();

        private async void CountInstallingMaps()
        {
            // This is meant to be called once for each map being installed
            // Once we count that we've installed them all we can do things
            numMapsInstalled++;
            Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = numMapsInstalled;
            });

            Log($"{Properties.Resources.str_Installed_Map} {numMapsInstalled} of {numMapsInstalling}");
            if (numMapsInstalled >= numMapsInstalling)
            {
                UpdateInstalledMaps();
                await CheckContents();
                Invoke((MethodInvoker)delegate
               {
                   progressBar1.Visible = false;
                   CheckMapUpdates();
               });
                numMapsInstalled = 0;
                numMapsInstalling = 0; // Just added these... probably not useful or necessary oh well
                if (numFailedMaps == 0)
                    Log(Properties.Resources.str_All_maps_installed_You_may_need_to);
                else
                    Log($"{numFailedMaps} maps failed to install, please retry");
                numFailedMaps = 0;
            }
        }

        private int numFailedMaps = 0;

        private int numMapsInstalling = 0;
        private int numMapsInstalled = 0;
        private object countingLocker = new Object();

        private void InstallButton_Click(object sender, EventArgs e)
        {
            // Download and unzip each map into MordhauPath
            // Then delete the zip to keep it clean
            numMapsInstalling = AvailableMapsBox.CheckedItems.Count;
            numMapsInstalled = 0;

            InstallButton.Enabled = false;
            int mapNum = 0;
            // Enable progress bars
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = numMapsInstalling;
            progressBar1.CustomText = $"{Properties.Resources.str_Beginning_install_for}{numMapsInstalling} maps...";

            Refresh();

            foreach (Map m in AvailableMapsBox.CheckedItems)
            {
                m.installNumber = mapNum;
                Log(Properties.Resources.str_Beginning_install_for + m.name);
                Task.Run(() =>
                {
                    InstallMap(m);
                    lock (countingLocker)
                        CountInstallingMaps();
                });
                mapNum++;
            }

            for (var i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, false);

        }

        private void UpdateInstalledMaps()
        {
            if (InvokeRequired)
            {
                var d = new SafeVoid(UpdateInstalledMaps);
                Invoke(d);
            }
            else
            {


                Map.installed = new List<Map>();
                // Find all files ending with .info.txt in mordhaupath
                foreach (string f in Directory.GetFiles(m_MordhauPath, "*.info.txt", SearchOption.AllDirectories))
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
                            m.releaseDate = DateTime.ParseExact(lines[MapVars.releaseDate], "dd/MM/yyyy",
                                CultureInfo.InvariantCulture);
                            m.fileSize = lines[MapVars.fileSize];
                            if (lines.Length > MapVars.suggestedPlayers)
                                m.suggestedPlayers = lines[MapVars.suggestedPlayers];
                            if (lines.Length > MapVars.thumbnailURL)
                                m.thumbnailURL = lines[MapVars.thumbnailURL];
                            if (lines.Length > MapVars.downloadURL && lines[MapVars.downloadURL].Length > 0)
                                m.downloadURL = GetDownloadLink(lines[MapVars.downloadURL]);
                            else
                                m.downloadURL =
                                    $@"https://github.com/MordhauMappingModding/MapFiles/blob/master/{m.folderName}.zip?raw=true";
                            //Log("Adding map " + m.name);
                            Map.installed.Add(m);
                        }
                        catch (Exception e)
                        {
                            Log($"Error in installed info file: {f}; {e.Message}");
                        }
                    }
                }

                CheckMapUpdates();

                InstalledMapsBox.DataSource = Map.installed;
                InstalledMapsBox.DisplayMember = "name";
                Update();
            }
        }

        private void CheckMapUpdates()
        {
            try
            {
                // Screw it, first remove all outdated and update tags
                foreach (Map m in Map.maps)
                {
                    m.name = m.name.Replace("[UPDATED] ", "");
                }

                foreach (Map m in Map.installed)
                {
                    m.name = m.name.Replace("[OUTDATED] ", "");
                    // While we're here, let's go ahead and remove any maps that are installed already
                    // I guess we'll move the updated/outdated logic here too
                    foreach (Map m2 in Map.maps)
                    {
                        if (m2.folderName.Equals(m.folderName))
                        {
                            if (!m2.version.Equals(m.version))
                            {
                                m.name = $"[OUTDATED] {m.name}";
                                m2.name = $"[UPDATED] {m2.name}";
                            }
                            else
                            {
                                Map.maps.Remove(m2);
                            }

                            break;
                        }
                    }
                }

                AvailableMapsBox.DataSource = Map.maps;
                AvailableMapsBox.DisplayMember = "name";
                //AvailableMapsBox.Refresh();
                //InstalledMapsBox.Refresh();
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.StackTrace);
                Log("Error checking map updates");
            }
        }

        private void SetProgressText(string s)
        {
            using (Graphics gr = progressBar1.CreateGraphics())
            {
                gr.DrawString(s, Font, new SolidBrush(ForeColor),
                    new PointF(Width / 2 - (gr.MeasureString(s, Font).Width / 2.0F),
                        Height / 2 - (gr.MeasureString(s, Font).Height / 2.0F)));
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates(true);
        }

        private void InstallMap(Map m)
        {
            try
            {
                Invoke((MethodInvoker) delegate { progressBar1.CustomText = Properties.Resources.str_Beginning_install_for + m.name; });
                // Let's pass in a map number to each of these
                // And it will loop and wait until mapnum-numMapsInstalled < maxSimultaneousMapDownloads
                // So map 11 waits until at least 1 map is installed, map 8 doesn't wait, etc
                /*
                Log($"Installing {m.name} as mapNum {m.installNumber}");
                while (m.installNumber - maxSimultaneousMapDownloads >= numMapsInstalled)
                {
                    Thread.Sleep(100);
                }
                */
                // This didn't fix 2cool's problem so let's not complicate it

                Log(Properties.Resources.str_Downloading_name_as_folderName_zip + $" {m.folderName}.zip");
                // https://github.com/MordhauMappingModding/MapFiles/blob/master/BerzerkerArena.zip
                // This fucking api can't handle downloading zip files so I have to do it by hand and hope we don't ratelimit
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(
                        m.downloadURL,
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
                File.Copy($@"{s_InfoFiles}{s_InfoFilesFolder}\{m.folderName}.info.txt",
                    $@"{m_MordhauPath}{m.folderName}\{m.folderName}.info.txt", true);
                Log(Properties.Resources.str_Successfully_installed_name + m.name);
                Invoke((MethodInvoker) delegate
                {
                    progressBar1.CustomText = Properties.Resources.str_Successfully_installed_name + m.name;
                });
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Log(e.InnerException.StackTrace);
                    Log(e.InnerException.Message);
                }
                Log(e.StackTrace);
                Log(Properties.Resources.str_Failed_during_installation_Message + e.Message);
                numFailedMaps++;
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int numMaps = InstalledMapsBox.CheckedItems.Count;
            foreach (Map m in InstalledMapsBox.CheckedItems)
            {
                try
                {
                    Directory.Delete($"{m_MordhauPath}{m.folderName}", true);
                    Log(Properties.Resources.str_Successfully_deleted_name + m.name);
                }
                catch (Exception ex)
                {
                    Log($"Error while deleting {m.name}: {ex.Message}\n{ex.StackTrace}");
                }
                try
                {
                    foreach (string f in Directory.GetFiles(m_MordhauPath, $@"{m.folderName}.info.txt", SearchOption.AllDirectories))
                    {
                        File.Delete(f);
                    }
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
            CheckContents();
            CheckMapUpdates();
            Log($"{numMaps} maps removed");
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
                CheckMapUpdates();
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }

            Update();
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
        public static int downloadURL = 9;
    }
}