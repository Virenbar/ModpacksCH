using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class Hash
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("algo")]
        public long Algo { get; set; }
    }
}