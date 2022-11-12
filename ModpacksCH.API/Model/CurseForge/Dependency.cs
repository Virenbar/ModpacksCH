using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class Dependency
    {
        [JsonProperty("modId")]
        public long ModId { get; set; }

        [JsonProperty("relationType")]
        public long RelationType { get; set; }
    }
}