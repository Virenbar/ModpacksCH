namespace ModpacksCH.API.Model
{
    public class ModpackManifest
    {
        public List<Art> Art { get; set; }
        public List<Author> Authors { get; set; }
        public string Description { get; set; }
        public bool Featured { get; set; }
        public int ID { get; set; }
        public int Installs { get; set; }
        public List<ManifestLink> Links { get; set; }
        public string Name { get; set; }
        public string Notification { get; set; }
        public int Plays { get; set; }
        public int Plays_14d { get; set; }
        public Rating Rating { get; set; }
        public long Refreshed { get; set; }
        public long Released { get; set; }
        public string Status { get; set; }
        public string Synopsis { get; set; }
        public List<Tag> Tags { get; set; }
        public string Type { get; set; }
        public long Updated { get; set; }
        public List<Version> Versions { get; set; }
    }
}