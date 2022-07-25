namespace ModpacksCH.API.Model
{
    public class CHManifest : ModpackManifest
    {
        public List<Author> Authors { get; set; }
        public int Installs { get; set; }
        public List<CHLink> Links { get; set; }
        public int Plays { get; set; }
        public int Plays_14d { get; set; }
        public long Released { get; set; }
        public List<Tag> Tags { get; set; }
    }
}