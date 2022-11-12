using ModpacksCH.API;
using ModpacksCH.API.Model;
using ModpacksCH.Models;
using System.Diagnostics;

namespace ModpacksCH
{
    public class ModpackDownloader : IDisposable
    {
        private readonly DownloadInfo Info;
        private readonly SemaphoreSlim Semaphore;
        private CFClient CF;
        private HttpClient Client;

        public ModpackDownloader(DownloadInfo info) : this(info, 4) { }

        public ModpackDownloader(DownloadInfo info, int threads)
        {
            Info = info;
            Semaphore = new(threads);
        }

        public Task Download() => Download(default);

        public async Task Download(IProgress<int> IP)
        {
            var Files = Info.Files;
            int Count = 0;
            Trace.WriteLine($"Download started: {Files.Count} files");
            using (Client = new())
            {
                var Tasks = Files.Select(async (F) =>
                {
                    var File = await TryDownloadTile(F);
                    Interlocked.Increment(ref Count);
                    IP.Report(Count);
                });
                await Task.WhenAll(Tasks);
                IP.Report(Count);
            }
            Trace.WriteLine($"Download done: {Files.Count} files");
            //TODO Add hash check
        }

        private async Task<string> TryDownloadTile(ModpackFile file)
        {
            await Semaphore.WaitAsync();

            var LocalFile = new FileInfo(Path.Combine(Info.OutPath, file.Path, file.Name));
            var URL = file.Url;
            if (string.IsNullOrEmpty(URL))
            {
                CF ??= new CFClient();
                var File = (await CF.GetModFile(file.CurseForge.Project, file.CurseForge.File)).Data;
                URL = File.DownloadURL;
                //IDK Why url is null
                if (URL is null)
                {
                    var ID = file.CurseForge.File.ToString();
                    URL = $"https://edge.forgecdn.net/files/{ID[..4]}/{ID[4..]}/{File.FileName}";
                }
            }

            Trace.WriteLine($"Downloading file: {URL} ");
            using var Responce = await Client.GetAsync(URL);
            Responce.EnsureSuccessStatusCode();
            Directory.CreateDirectory(LocalFile.DirectoryName);
            using var FS = new FileStream(LocalFile.FullName, FileMode.Create);
            await Responce.Content.CopyToAsync(FS);
            Semaphore.Release();
            return LocalFile.FullName;
        }

        #region IDispose

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
            ((IDisposable)CF).Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion IDispose
    }
}