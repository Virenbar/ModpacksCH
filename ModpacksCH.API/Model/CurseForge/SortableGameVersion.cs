using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class SortableGameVersion
    {
        [JsonProperty("gameVersionName")]
        public string GameVersionName { get; set; }

        [JsonProperty("gameVersionPadded")]
        public string GameVersionPadded { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("gameVersionReleaseDate")]
        public DateTimeOffset GameVersionReleaseDate { get; set; }

        [JsonProperty("gameVersionTypeId")]
        public long GameVersionTypeId { get; set; }
    }
}