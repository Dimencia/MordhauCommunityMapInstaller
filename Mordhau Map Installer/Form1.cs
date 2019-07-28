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
using Octokit;
using Application = System.Windows.Forms.Application;

namespace Mordhau_Map_Installer
{
    public partial class Form1 : Form
    {
        private const string MAPS_PATH = @"steamapps\common\mordhau\mordhau\content\mordhau\maps\", VERSION = "1.0.0.21";

        private static readonly string
            s_ApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            s_AppData = $@"{s_ApplicationDataPath}\MordhauMapInstaller";

        private readonly Image m_DefaultThumbnail;
        private string m_MordhauPath = string.Empty;
        private int m_RequestsLeft = 6000;

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

            // If not, try to auto-detect
            // If unsuccessful, prompt user to browse to it
            // If either, save ini file with location after

            // Then download all .txt files from github
            // First find them.  
            UpdateInstalledMaps(); // And list any installed maps
            Log("Checking for maps...");
            CheckContents("MordhauMappingModding", "MapFiles", @"\");

            // We do a zipball, unzip it, and check the files
            // When we want to download, we do: https://github.com/MordhauMappingModding/MapFiles/blob/master/BerzerkerArena.zip?raw=true for example

            // List each map by map name in available maps
            // Check install directory for any .info.txt files and list in installed

            // Then wait for a button click, install or remove.  Check appropriate boxes for checkmarks, install or remove as necessary
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
            UpdateMapInfo(((CheckedListBox) sender).SelectedValue as Map);
            InstallButton.Enabled = AvailableMapsBox.CheckedItems.Count > 0;
            RemoveButton.Enabled = InstalledMapsBox.CheckedItems.Count > 0;
            if (m_RequestsLeft <= 0)
                Log("No requests left for community account!  Swapping to unauthenticated.  Only 60 requests per user");
            if (m_MordhauPath.ToLower().Contains(@"steamapps\common\mordhau\mordhau\content\mordhau\maps")) return;
            RemoveButton.Enabled = false;
            InstallButton.Enabled = false;
            Log("Invalid Mordhau path!  Re-set it in Settings before doing anything");
        }

        private void UpdateMapInfo(Map map)
        {
            if (map == null) return;
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

        private async Task CheckContents(string repoOwner, string repoName, string path)
        {
            Map.maps = new List<Map>();
            var client = new GitHubClient(new ProductHeaderValue("Mordhau-Map-Installer"));
            // This access token has absolutely no permissions and just lets us bypass rate limits.  Good luck using it for anything
            if (m_RequestsLeft > 0)
                client.Credentials = new Credentials("CommunityMapBot", "040ba3075bf2bb0c6581874bfc687a7b2691e7cc");
            // Set credentials here, otherwise harsh rate limits apply.
            Log("Getting all contents");
            IReadOnlyList<RepositoryContent> contents = await client.Repository.Content.GetAllContents(repoOwner, repoName, path);
            IEnumerable<string> results = contents.Select(content => content.Name);

            ApiInfo apiInfo = client.GetLastApiInfo();
            m_RequestsLeft = apiInfo?.RateLimit?.Remaining ?? 6000;
            Log($"{m_RequestsLeft} requests left this hour");

            Directory.CreateDirectory($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\");
            Log("Checking resulting files");
            foreach (string r in results)
            {
                // Anything ending in .info.txt
                if (!r.ToLower().EndsWith(".info.txt")) continue;
                IReadOnlyList<RepositoryContent> resources = await client.Repository.Content.GetAllContents(repoOwner, repoName, $@"\{r}");
                // Let's save it so we can copy it in later if we DL the map
                using (var writer = new StreamWriter($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{r}"))
                    writer.Write(resources[0].Content);
                // So now resource[0].Content is our info file, let's create an object and put info in it
                var m = new Map();
                try
                {
                    string[] lines = resources[0].Content.Split('\n');
                    m.name = lines[0];
                    m.folderName = lines[1];
                    m.description = lines[2];
                    m.authors = lines[3];
                    m.version = lines[4];
                    m.releaseDate = DateTime.ParseExact(lines[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    m.fileSize = lines[6];
                    if (lines.Length > 7)
                        m.suggestedPlayers = lines[7];
                    if (lines.Length > 8)
                        m.thumbnailURL = lines[8];
                    //Log("Adding map " + m.name);
                    Map.maps.Add(m);
                    foreach (Map y in Map.installed.Where(z =>
                        z.folderName.Equals(m.folderName) && !z.version.Equals(m.version) && !z.needsUpdate))
                    {
                        y.needsUpdate = true;
                        y.name = $"[OUTDATED] {y.name}";
                        InstalledMapsBox.Refresh();
                    }
                }
                catch (Exception e)
                {
                    Log($"Error in info file: {r}; {e.Message}");
                }
            }

            Log("Updating available maps");
            UpdateAvailableMaps();
            Update();
            Log("Ready");
        }

        private void UpdateAvailableMaps()
        {
            Log($"{Map.maps.Count} maps being updated");
            AvailableMapsBox.Items.Clear();
            AvailableMapsBox.DataSource = Map.maps;
            AvailableMapsBox.DisplayMember = "name";
            Log($"{Map.maps.Count} maps updated");
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
            // Check 64 bit registry first
            /*
            RegistryKey localKey =
                RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
                    RegistryView.Registry32);
            RegistryKey sqlServerKey = localKey.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 629760");
            string steamPath = (string)sqlServerKey.GetValue("InstallLocation");
            */
            // The above returns null for the localkey...
            var steamPath = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 629760", "InstallLocation", string.Empty);
            if (steamPath != null)
                Log("Successfully read from Mordhau InstallLocation");
            if (string.IsNullOrEmpty(steamPath))
                steamPath = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", string.Empty);
            if (string.IsNullOrEmpty(steamPath))
                steamPath = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath",
                    steamPath);

            return steamPath ?? string.Empty;
        }

        private static void Scan(string path)
        {
            try
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.exe"))
                {
                    Console.WriteLine($@"FILE: {file}");
                }
                foreach (string dir in Directory.EnumerateDirectories(path))
                {
                    Console.WriteLine($@"DIRECTORY: {dir}" );
                    Scan(dir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($@"Error: {path}");
            }
        }

        private void setMordhauDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMordhauPathDialog();
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            // Download and unzip each map into MordhauPath
            // Then delete the zip to keep it clean
            foreach (Map m in AvailableMapsBox.CheckedItems)
            {
                Log("Beginning install for " + m.name);
                InstallMap(m);
            }

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
                        string[] lines = reader.ReadToEnd().Split('\n');
                        m.name = lines[0];
                        m.folderName = lines[1];
                        m.description = lines[2];
                        m.authors = lines[3];
                        m.version = lines[4];
                        m.releaseDate = DateTime.ParseExact(lines[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        m.fileSize = lines[6];
                        if (lines.Length > 7)
                            m.suggestedPlayers = lines[7];
                        if (lines.Length > 8)
                            m.thumbnailURL = lines[8];
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
                    Log($"{m_RequestsLeft} requests left this hour");
                    if (m_RequestsLeft > 0)
                        wc.Credentials = new NetworkCredential("CommunityMapBot", "040ba3075bf2bb0c6581874bfc687a7b2691e7cc");
                    wc.DownloadFile(
                        $@"https://github.com/MordhauMappingModding/MapFiles/blob/master/{m.folderName}.zip?raw=true",
                        $@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{m.folderName}.zip");
                    m_RequestsLeft--;
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
                File.Copy($@"{s_ApplicationDataPath}\MordhauMapInstaller\Info\{m.folderName}.info.txt",
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
                UpdateInstalledMaps(); // And list any installed maps
                Log("Checking for maps...");
                CheckContents("MordhauMappingModding", "MapFiles", @"\");
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        private void basicHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var help = new HelpForm();
            help.Show();
        }
    }
}