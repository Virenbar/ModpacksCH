using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class ModFileResponse
    {
        [JsonProperty("data")]
        public ModFile Data { get; set; }
    }

    public class ModFile
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("gameId")]
        public long GameId { get; set; }

        [JsonProperty("modId")]
        public long ModId { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("releaseType")]
        public long ReleaseType { get; set; }

        [JsonProperty("fileStatus")]
        public long FileStatus { get; set; }

        [JsonProperty("hashes")]
        public Hash[] Hashes { get; set; }

        [JsonProperty("fileDate")]
        public DateTimeOffset FileDate { get; set; }

        [JsonProperty("fileLength")]
        public long FileLength { get; set; }

        [JsonProperty("downloadCount")]
        public long DownloadCount { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadURL { get; set; }

        [JsonProperty("gameVersions")]
        public string[] GameVersions { get; set; }

        [JsonProperty("sortableGameVersions")]
        public SortableGameVersion[] SortableGameVersions { get; set; }

        [JsonProperty("dependencies")]
        public Dependency[] Dependencies { get; set; }

        [JsonProperty("exposeAsAlternative")]
        public bool ExposeAsAlternative { get; set; }

        [JsonProperty("parentProjectFileId")]
        public long ParentProjectFileID { get; set; }

        [JsonProperty("alternateFileId")]
        public long AlternateFileID { get; set; }

        [JsonProperty("isServerPack")]
        public bool IsServerPack { get; set; }

        [JsonProperty("serverPackFileId")]
        public long ServerPackFileID { get; set; }

        [JsonProperty("fileFingerprint")]
        public long FileFingerprint { get; set; }

        [JsonProperty("modules")]
        public Module[] Modules { get; set; }
    }
}