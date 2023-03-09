namespace ModpacksCH.API.Model
{
    public class CHVersion : VersionManifest
    {
        public Specs Specs { get; set; }
        public bool Private { get; set; }
    }
}