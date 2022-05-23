namespace ModpacksCH.API.Model
{
    public class SearchResult
    {
        public List<int> CurseForge { get; set; } = new();
        public int Limit { get; set; }
        public List<int> Packs { get; set; } = new();
        public long Refreshed { get; set; }
        public int Total { get; set; }
    }
}