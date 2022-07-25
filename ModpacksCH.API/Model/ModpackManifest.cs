namespace ModpacksCH.API.Model
{
    public class ModpackManifest
    {
        public List<Art> Art { get; set; }
        public string Description { get; set; }
        public bool Featured { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notification { get; set; }
        public Rating Rating { get; set; }
        public long Refreshed { get; set; }
        public string Status { get; set; }
        public string Synopsis { get; set; }
        public string Type { get; set; }
        public long Updated { get; set; }
        public List<Version> Versions { get; set; }
    }
}