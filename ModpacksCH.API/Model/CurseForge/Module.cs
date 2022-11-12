using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class Module
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fingerprint")]
        public long Fingerprint { get; set; }
    }
}