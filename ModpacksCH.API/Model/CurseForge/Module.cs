using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public partial class Module
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fingerprint")]
        public long Fingerprint { get; set; }
    }
}