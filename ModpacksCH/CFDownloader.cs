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

        public override async Task<DownloadResult> Download(string path, IProgress<int> IP)
        {
            Trace.WriteLine($"Download manifest: {Temp.FullName}");
            var archive = Info.Files.First(F => F.Name == "overrides.zip");
            Info.Files.Remove(archive);
            var archivePath = await DownloadFile(Temp.FullName, archive);
            ZipFile.ExtractToDirectory(archivePath, Temp.FullName, true);
            Count++;

            var manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(Path.Combine(Temp.FullName, "manifest.json")));
            var modpackName = $"{Info.Modpack.Name} - {manifest.Version}{(Info.IsServer ? "(server)" : "")}";
            var modpackPath = Path.Combine(path, modpackName);
            Trace.WriteLine($"Modpack: {modpackName}");

            var overrides = new DirectoryInfo(Path.Combine(Temp.FullName, manifest.Overrides));
            var files = overrides.EnumerateFiles("*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(overrides.FullName, file.FullName);
                var filePath = new FileInfo(Path.Combine(modpackPath, relativePath));
                filePath.Directory.Create();

                file.MoveTo(filePath.FullName, true);
            }
            Temp.Delete(true);
            Trace.WriteLine($"Manifest and Overrides done");
            var (_, Errors) = await base.Download(modpackPath, IP);
            return new(modpackPath, Errors);
        }
    }
}