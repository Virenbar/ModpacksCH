using ModpacksCH.API.Model;

namespace ModpacksCH.Models
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
            Files = Version.Files.Where(F => IsServer ? !F.ClientOnly : !F.ServerOnly).ToList();
        }

        public List<ModpackFile> Files { get; }
        public bool IsServer { get; }
        public ModpackManifest Modpack { get; }
        public string OutPath { get; }
        public VersionManifest Version { get; }
    }
}