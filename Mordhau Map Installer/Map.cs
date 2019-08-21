using System;
using System.Collections.Generic;

namespace Mordhau_Map_Installer
{
    public class Map
    {
        public static List<Map> maps = new List<Map>();
        public static List<Map> installed = new List<Map>();
        
        public string name { get; set; }
        public string description;
        public string folderName;
        public string authors;
        public string version;
        public DateTime releaseDate;
        public string fileSize;
        public string suggestedPlayers = "";
        public bool needsUpdate = false;
        public string thumbnailURL = "";
        public string downloadURL = "";

        public int installNumber = 0;
    }
}