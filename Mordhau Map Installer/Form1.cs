using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Mordhau_Map_Installer
{
    public partial class Form1 : Form
    {
        private string MordhauPath = "";

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


            string version = "";
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
            }

            Log("Version " + version);
            versionlabel.Text = "Version " + version;

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller");

            // Remove old log file so we can make a new one
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                           @"\MordhauMapInstaller\log.txt"))
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            @"\MordhauMapInstaller\log.txt");

            // Next check if we have an ini file with mordhau location
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller";


            if (File.Exists(localPath + @"\config.txt"))
            {
                using (StreamReader reader = new StreamReader(localPath + @"\config.txt"))
                    MordhauPath = reader.ReadLine();
                Log("Mordhau Path read from configuration file");
            }
            else
            {
                // If no file, show help form
                HelpForm help = new HelpForm();
                help.ShowDialog();
            }
            // Even if we loaded one, make sure it's right, if not loaded this will also be true
            if (!MordhauPath.ToLower().Contains(@"steamapps\common\mordhau\mordhau\content\mordhau\maps\") || !Directory.Exists(MordhauPath))
            {
                Log("Trying to find Mordhau Directory...");
                string steamPath = GetSteamPath();
                Log("Steam Path: " + steamPath); // If steampath contains mordhau, it's the straightup mordhau path
                if (steamPath.Equals("") || (!Directory.Exists(steamPath + @"\steamapps\common\mordhau") && !steamPath.ToLower().Contains("mordhau")))
                {
                    Log("Mordhau not found");
                    // Open configuration menu to have them browse to it
                    ShowMordhauPathDialog();
                    if (!MordhauPath.ToLower().Contains(@"steamapps\common\mordhau\mordhau\content\mordhau\maps\") || !Directory.Exists(MordhauPath))
                    {
                        Log("Invalid Mordhau Path, please re-enter through Settings: " + MordhauPath);
                    }
                    Log("Mordhau Path: " + MordhauPath);
                }
                else
                {
                    if (steamPath.ToLower().Contains("mordhau")) // Mordhau directory, just add the other stuff
                        MordhauPath = steamPath + @"\mordhau\content\mordhau\maps\";
                    else
                        MordhauPath = steamPath + @"\steamapps\common\mordhau\mordhau\content\mordhau\maps\";
                    Log("Mordhau Path: " + MordhauPath);
                }
                // Regardless let's save now
                using (StreamWriter writer = new StreamWriter(localPath + @"\config.txt"))
                {
                    writer.WriteLine(MordhauPath);
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
            for (int i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, false);
            RemoveButton.Enabled = false;
        }

        private void SelectAllToolStripMenuItem1OnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, true);
            RemoveButton.Enabled = true;
        }

        private void SelectNoneToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, false);
            InstallButton.Enabled = false;
        }

        private void SelectAllToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            for(int i = 0; i < AvailableMapsBox.Items.Count; i++)
                AvailableMapsBox.SetItemChecked(i, true);
            InstallButton.Enabled = true;
        }

        private void MapDescriptionLabelOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            if (target != null && target.ToLower().StartsWith("http"))
                System.Diagnostics.Process.Start(target);
        }

        private void AvailableMapsBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            UpdateMapInfo(((CheckedListBox)sender).SelectedValue as Map);
            if (AvailableMapsBox.CheckedItems.Count > 0)
                InstallButton.Enabled = true;
            else
                InstallButton.Enabled = false;
            if (InstalledMapsBox.CheckedItems.Count > 0)
                RemoveButton.Enabled = true;
            else
                RemoveButton.Enabled = false;

            if (!MordhauPath.ToLower().Contains(@"steamapps\common\mordhau\mordhau\content\mordhau\maps"))
            {
                RemoveButton.Enabled = false;
                InstallButton.Enabled = false;
                Log("Invalid Mordhau path!  Re-set it in Settings before doing anything");
            }
        }

        private void UpdateMapInfo(Map map)
        {
            if (map != null)
            {
                MapNameLabel.Text = map.folderName;
                MapDescriptionLabel.Links.Clear();
                MapDescriptionLabel.Text = map.description;


                string desc = map.description;
                int lastEnd = 0;
                while (desc.ToLower().Contains("http"))
                {
                    // Find the http, and end of it, and mark beginning and end with linkarea
                    int beginning = desc.ToLower().IndexOf("http");
                    desc = desc.Substring(beginning);
                    beginning += lastEnd; // To account for last movement
                    int end = desc.IndexOf(" ");
                    if (end == -1)
                        end = desc.Length;
                    int fakeend = end + beginning; // This goes into linkarea
                    lastEnd = fakeend + 1;
                    string link = map.description.Substring(beginning, (fakeend - beginning));
                    //Log("Adding link region: " + beginning + ", " + fakeend);
                    try
                    {
                        MapDescriptionLabel.Links.Add(beginning, fakeend - beginning, link);
                    }
                    catch (Exception)
                    {
                    }
                    // And we preserved end so we can now truncate to ignore it in next search
                    desc = desc.Substring(end + 1);
                }
                MapAuthorsLabel.Text = map.authors;
                MapVersionLabel.Text = map.version;
                MapReleaseDateLabel.Text = map.releaseDate.ToLongDateString();
                MapSizeLabel.Text = map.fileSize;
                MapSuggestedPlayersLabel.Text = map.suggestedPlayers;
            }

        }

        private async Task CheckContents(string repoOwner, string repoName, string path)
        {
            Map.maps = new List<Map>();
            var client = new GitHubClient(new ProductHeaderValue("Github-API-Test"));
            // This access token has absolutely no permissions and just lets us bypass rate limits.  Good luck using it for anything
            client.Credentials = new Credentials("CommunityMapBot", "040ba3075bf2bb0c6581874bfc687a7b2691e7cc");
            // Set credentials here, otherwise harsh rate limits apply.

            var contents = await client.Repository.Content.GetAllContents(repoOwner, repoName, path);
            var results = contents.Select(content => content.Name);

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\"))
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\");
            foreach (var r in results)
            {
                // Anything ending in .info.txt
                if (r.ToLower().EndsWith(".info.txt"))
                {
                    var resource = await client.Repository.Content.GetAllContents(repoOwner, repoName, @"\" + r);
                    // Let's save it so we can copy it in later if we DL the map
                    using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\" + r))
                        writer.Write(resource[0].Content);
                    // So now resource[0].Content is our info file, let's create an object and put info in it
                    Map m = new Map();
                    try
                    {
                        string[] lines = resource[0].Content.Split('\n');
                        m.name = lines[0];
                        m.folderName = lines[1];
                        m.description = lines[2];
                        m.authors = lines[3];
                        m.version = lines[4];
                        m.releaseDate = DateTime.ParseExact(lines[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        m.fileSize = lines[6];
                        if (lines.Length > 7)
                            m.suggestedPlayers = lines[7];
                        //Log("Adding map " + m.name);
                        Map.maps.Add(m);
                        foreach (Map y in Map.installed.Where(z =>
                            z.folderName.Equals(m.folderName) && !z.version.Equals(m.version) && !z.needsUpdate))
                        {
                            y.needsUpdate = true;
                            y.name = "[OUTDATED] " + y.name;
                            InstalledMapsBox.Refresh();
                        }
                    }
                    catch (Exception e)
                    {
                        Log("Error in info file: " + r + "; " + e.Message);
                    }
                }
            }
            UpdateAvailableMaps();
            Update();
            Log("Ready");
        }

        private void UpdateAvailableMaps()
        {
            AvailableMapsBox.Items.Clear();
            AvailableMapsBox.DataSource = Map.maps;
            AvailableMapsBox.DisplayMember = "name";
        }

        private void ShowMordhauPathDialog()
        {
            var browseForm = new BrowseForm();
            var result = browseForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!browseForm.getResultText().ToLower().Contains("mordhau") || !Directory.Exists(browseForm.getResultText()))
                {
                    MordhauPath = "";
                    Log("Invalid Mordhau Path!  Re-set through Settings");
                }
                else
                {
                    MordhauPath = browseForm.getResultText() + @"\mordhau\content\mordhau\maps\";
                    Log("New Mordhau Path: " + MordhauPath);
                }
            }
            browseForm.Dispose();
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
            labelspacer.Text = message;
            using (StreamWriter writer =
                File.AppendText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                @"\MordhauMapInstaller\log.txt"))
            {
                writer.WriteLine(message);
            }
        }

        private string GetSteamPath()
        {
            string steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 629760", "InstallLocation", "");
            if (steamPath == null)
                steamPath = "";
            Log("Uninstall path: " + steamPath);
            if (steamPath.Equals(""))
                steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", "");
            if (steamPath.Equals(""))
                steamPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath",
                    steamPath);
            return steamPath;
        }

        private static void Scan(string path)
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.exe"))
                {
                    Console.WriteLine("FILE: " + file);
                }
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    Console.WriteLine("DIRECTORY: " + dir);
                    Scan(dir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: " + path);
            }

        }

        private void setMordhauDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMordhauPathDialog();
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            var client = new GitHubClient(new ProductHeaderValue("Github-API-Test"));
            client.Credentials = new Credentials("CommunityMapBot", "040ba3075bf2bb0c6581874bfc687a7b2691e7cc");
            // Set credentials here, otherwise harsh rate limits apply.

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
            foreach (string f in Directory.GetFiles(MordhauPath, "*.info.txt"))
            {
                using (StreamReader reader = new StreamReader(f))
                {
                    Map m = new Map();
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
                        //Log("Adding map " + m.name);
                        Map.installed.Add(m);
                    }
                    catch (Exception e)
                    {
                        Log("Error in installed info file: " + f + "; " + e.Message);
                    }
                }
            }
            foreach (Map m in Map.maps)
            {
                foreach (Map y in Map.installed.Where(z =>
                    z.folderName.Equals(m.folderName) && !z.version.Equals(m.version) && !z.needsUpdate))
                {
                    y.needsUpdate = true;
                    y.name = "[OUTDATED] " + y.name;
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
                Log("Downloading " + m.name + " as " + @"\" + m.folderName + ".zip");
                // https://github.com/MordhauMappingModding/MapFiles/blob/master/BerzerkerArena.zip
                // This fucking api can't handle downloading zip files so I have to do it by hand and hope we don't ratelimit
                using (WebClient wc = new WebClient())
                {
                    wc.Credentials = new NetworkCredential("CommunityMapBot", "040ba3075bf2bb0c6581874bfc687a7b2691e7cc");
                    wc.DownloadFile(
                        @"https://github.com/MordhauMappingModding/MapFiles/blob/master/" + m.folderName +
                        ".zip?raw=true", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\" + m.folderName + ".zip");
                }

                // Check if folder already exists and delete if it does
                if (Directory.Exists(MordhauPath + m.folderName))
                    Directory.Delete(MordhauPath + m.folderName, true);
                Log("Extracting " + m.name);
                ZipFile.ExtractToDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\" + m.folderName + ".zip",
                    MordhauPath);
                Log("Deleting " + m.name);
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\" + m.folderName + ".zip");
                // Copy over the info file
                File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MordhauMapInstaller" + @"\Info\" + m.folderName + ".info.txt", MordhauPath + m.folderName + ".info.txt", true);
                Log("Successfully installed " + m.name);
            }
            catch (Exception e)
            {
                Log("Failed during installation: " + e.Message);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (Map m in InstalledMapsBox.CheckedItems)
            {
                try
                {
                    Directory.Delete(MordhauPath + m.folderName, true);
                    Log("Successfully deleted " + m.name);
                }
                catch (Exception ex)
                {
                    Log("Error while deleting " + m.name + ": " + ex.Message + "\n" + ex.StackTrace);
                }
                try
                {
                    File.Delete(MordhauPath + m.folderName + ".info.txt");
                }
                catch (Exception ex)
                {
                    Log("Error while deleting " + m.name + ": " + ex.Message + "\n" + ex.StackTrace);
                }
            }
            for (int i = 0; i < InstalledMapsBox.Items.Count; i++)
                InstalledMapsBox.SetItemChecked(i, false);
            RemoveButton.Enabled = false;
            UpdateInstalledMaps();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
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
                Log("Error: " + ex.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void basicHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.ShowDialog();
        }
    }
}
