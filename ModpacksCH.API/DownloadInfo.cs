using ModpacksCH.API.Model;

namespace ModpacksCH.API
{
    public class DownloadInfo
    {
        public DownloadInfo(ModpackManifest modpack, VersionManifest version, string outPath) : this(modpack, version, outPath, false) { }

        public DownloadInfo(ModpackManifest modpack, VersionManifest version, string outPath, bool isServer)
        {
            Modpack = modpack;
            Version = version;
            OutPath = outPath;
            IsServer = isServer;
        }

        public bool IsServer { get; set; }
        public ModpackManifest Modpack { get; set; }
        public string OutPath { get; set; }
        public VersionManifest Version { get; set; }
    }
}