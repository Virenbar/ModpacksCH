namespace ModpacksCH.API.Model
{
    public class VersionManifest : Version
    {
        public string Changelog { get; set; }
        public List<ModpackFile> Files { get; set; }

        //public int ID { get; set; }
        public int Installs { get; set; }

        public List<object> Links { get; set; }

        //public string Name { get; set; }
        public string Notification { get; set; }

        public int Parent { get; set; }
        public int Plays { get; set; }
        public long Refreshed { get; set; }

        //public Specs Specs { get; set; }
        public string Status { get; set; }

        //public List<Target> Targets { get; set; }
        //public string Type { get; set; }
        //public long Updated { get; set; }
    }
}