using ModpacksCH.Models;

namespace ModpacksCH
{
    internal class CHDownloader : ModpackDownloader
    {
        public CHDownloader(DownloadInfo info) : base(info) { }

        public CHDownloader(DownloadInfo info, int threads) : base(info, threads) { }

        public override Task<DownloadResult> Download(string path, IProgress<int> IP)
        {
            var modpackName = $"{Info.Modpack.Name} - {Info.Version.Name}{(Info.IsServer ? "(server)" : "")}";
            var modpackPath = Path.Combine(path, modpackName);
            return base.Download(modpackPath, IP);
        }
    }
}