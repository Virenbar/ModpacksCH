namespace ModpacksCH.API.Model
{
    public class Art
    {
        public bool Compressed { get; set; }
        public int? Height { get; set; }
        public long ID { get; set; }
        public List<string> Mirrors { get; set; }
        public string SHA1 { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public long Updated { get; set; }
        public string URL { get; set; }
        public int? Width { get; set; }
    }
}