using ModpacksCH.Models;
using ModpacksCH.Models.CurseForge;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;

namespace ModpacksCH
{
    internal class CFDownloader : ModpackDownloader
    {
        private readonly DirectoryInfo Temp = new(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

        public CFDownloader(DownloadInfo info) : base(info) { }

        public CFDownloader(DownloadInfo info, int threads) : base(info, threads) { }

        public override async Task<string> Download(string path, IProgress<int> IP)
        {
            Trace.WriteLine($"Download manifest: {Temp.FullName}");
            var Archive = Info.Files.First(F => F.Name == "overrides.zip");
            Info.Files.Remove(Archive);
            var ArchivePath = await DownloadFile(Temp.FullName, Archive);
            Count++;

            ZipFile.ExtractToDirectory(ArchivePath, Temp.FullName, true);
            var Manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(Path.Combine(Temp.FullName, "manifest.json")));
            var ModpackName = $"{Info.Modpack.Name} - {Manifest.Version}{(Info.IsServer ? "(server)" : "")}";
            var ModpackPath = Path.Combine(path, ModpackName);
            Trace.WriteLine($"Modpack: {ModpackName}");

            var Overrides = new DirectoryInfo(Path.Combine(Temp.FullName, Manifest.Overrides));
            var Files = Overrides.EnumerateFiles("*", SearchOption.AllDirectories);
            foreach (var file in Files)
            {
                var FF = Path.GetRelativePath(Overrides.FullName, file.FullName);
                var Target = new FileInfo(Path.Combine(ModpackPath, FF));
                Target.Directory.Create();

                file.MoveTo(Target.FullName, true);
            }
            Temp.Delete(true);
            Trace.WriteLine($"Manifest and Overrides done");
            ModpackPath = await base.Download(ModpackPath, IP);
            return ModpackPath;
        }
    }
}