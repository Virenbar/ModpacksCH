using Newtonsoft.Json;

namespace ModpacksCH.Models.CurseForge
{
    internal class Manifest
    {
        [JsonProperty("minecraft")]
        public ManifestMinecraft Minecraft { get; set; }

        [JsonProperty("manifestType")]
        public string ManifestType { get; set; }

        [JsonProperty("manifestVersion")]
        public long ManifestVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("files")]
        public List<ManifestFile> Files { get; set; }

        [JsonProperty("overrides")]
        public string Overrides { get; set; }
    }
    internal class ManifestFile
    {
        [JsonProperty("projectID")]
        public long ProjectID { get; set; }

        [JsonProperty("fileID")]
        public long FileID { get; set; }

        [JsonProperty("required")]
        public bool FileRequired { get; set; }
    }

    internal class ManifestMinecraft
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("modLoaders")]
        public List<ManifestModLoader> ModLoaders { get; set; }
    }

    internal class ManifestModLoader
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }
}