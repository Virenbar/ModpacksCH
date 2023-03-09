namespace ModpacksCH.API.Model
{
    public class VersionManifest : Version
    {
        public string Changelog { get; set; }
        public List<ModpackFile> Files { get; set; }
        public int Installs { get; set; }
        public List<ManifestLink> Links { get; set; }
        public string Notification { get; set; }
        public int Parent { get; set; }
        public int Plays { get; set; }
        public long Refreshed { get; set; }
        public string Status { get; set; }
    }
}