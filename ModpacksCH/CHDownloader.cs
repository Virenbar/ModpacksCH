using ModpacksCH.Models;

namespace ModpacksCH
{
    internal class CHDownloader : ModpackDownloader
    {
        public CHDownloader(DownloadInfo info) : base(info) { }

        public CHDownloader(DownloadInfo info, int threads) : base(info, threads) { }

        public override Task<string> Download(string path, IProgress<int> IP = null)
        {
            var ModpackName = $"{Info.Modpack.Name} - {Info.Version.Name}{(Info.IsServer ? "(server)" : "")}";
            var ModpackPath = Path.Combine(path, ModpackName);
            return base.Download(ModpackPath, IP);
        }
    }
}