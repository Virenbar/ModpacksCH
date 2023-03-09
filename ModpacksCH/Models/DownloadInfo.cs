using ModpacksCH.API.Model;

namespace ModpacksCH.Models
{
    public class DownloadInfo
    {
        public DownloadInfo(ModpackManifest modpack, VersionManifest version) : this(modpack, version, false) { }

        public DownloadInfo(ModpackManifest modpack, VersionManifest version, bool isServer)
        {
            Modpack = modpack;
            Version = version;
            IsServer = isServer;
            Files = Version.Files.Where(F => IsServer ? !F.ClientOnly : !F.ServerOnly).ToList();
        }

        public List<ModpackFile> Files { get; }
        public bool IsServer { get; }
        public ModpackManifest Modpack { get; }
        public VersionManifest Version { get; }
    }
}