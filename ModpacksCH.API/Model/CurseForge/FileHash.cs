using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class FileHash
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("algo")]
        public FileAlgo Algo { get; set; }
    }
}