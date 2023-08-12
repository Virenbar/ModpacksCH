using Newtonsoft.Json;

namespace ModpacksCH.API.Model.CurseForge
{
    public class ModFileResponse
    {
        [JsonProperty("data")]
        public File Data { get; set; }
    }

    public class File
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("gameId")]
        public int GameId { get; set; }

        [JsonProperty("modId")]
        public int ModId { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("releaseType")]
        public FileReleaseType ReleaseType { get; set; }

        [JsonProperty("fileStatus")]
        public FileStatus FileStatus { get; set; }

        [JsonProperty("hashes")]
        public FileHash[] Hashes { get; set; }

        [JsonProperty("fileDate")]
        public DateTimeOffset FileDate { get; set; }

        [JsonProperty("fileLength")]
        public long FileLength { get; set; }

        [JsonProperty("downloadCount")]
        public long DownloadCount { get; set; }

        [JsonProperty("fileSizeOnDisk")]
        public long? FileSizeOnDisk { get; set; }

        [JsonProperty("downloadUrl")]
        public string DownloadURL { get; set; }

        [JsonProperty("gameVersions")]
        public string[] GameVersions { get; set; }

        [JsonProperty("sortableGameVersions")]
        public SortableGameVersion[] SortableGameVersions { get; set; }

        [JsonProperty("dependencies")]
        public FileDependency[] Dependencies { get; set; }

        [JsonProperty("exposeAsAlternative")]
        public bool? ExposeAsAlternative { get; set; }

        [JsonProperty("parentProjectFileId")]
        public int? ParentProjectFileID { get; set; }

        [JsonProperty("alternateFileId")]
        public int? AlternateFileID { get; set; }

        [JsonProperty("isServerPack")]
        public bool? IsServerPack { get; set; }

        [JsonProperty("serverPackFileId")]
        public int? ServerPackFileID { get; set; }

        [JsonProperty("fileFingerprint")]
        public long FileFingerprint { get; set; }

        [JsonProperty("modules")]
        public FileModule[] Modules { get; set; }
    }
}