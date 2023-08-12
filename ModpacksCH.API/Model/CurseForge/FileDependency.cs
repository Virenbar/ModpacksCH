using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class FileDependency
    {
        [JsonProperty("modId")]
        public int ModId { get; set; }

        [JsonProperty("relationType")]
        public FileRelationType RelationType { get; set; }
    }
}