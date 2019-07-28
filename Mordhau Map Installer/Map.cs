using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mordhau_Map_Installer
{
    public class Map
    {
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

        public static List<Map> maps = new List<Map>();
        public static List<Map> installed = new List<Map>();
    }


}
