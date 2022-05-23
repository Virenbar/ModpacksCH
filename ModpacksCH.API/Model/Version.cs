namespace ModpacksCH.API.Model
{
    public class Version
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Specs Specs { get; set; }
        public List<Target> Targets { get; set; }
        public string Type { get; set; }
        public long Updated { get; set; }
    }
}