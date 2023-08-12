using ModpacksCH.API;
using ModpacksCH.API.Model;
using ModpacksCH.Models;
using System.Diagnostics;

namespace ModpacksCH
{
    public abstract class ModpackDownloader : IDisposable
    {
        protected readonly DownloadInfo Info;
        protected readonly SemaphoreSlim Semaphore;
        protected int Count;
        private const string CDNEndpoint = "https://edge.forgecdn.net/files";
        private readonly CFClient CF = new();
        private readonly HttpClient Client = new();

        protected ModpackDownloader(DownloadInfo info) : this(info, 4) { }

        protected ModpackDownloader(DownloadInfo info, int threads)
        {
            Info = info;
            Semaphore = new(threads);
        }

        public static ModpackDownloader Create(DownloadInfo info)
        {
            return info.Version switch
            {
                CHVersion => new CHDownloader(info),
                CFVersion => new CFDownloader(info),
                _ => throw new NotImplementedException()
            };
        }

        public virtual Task<DownloadResult> Download(string path) => Download(path, default);

        public virtual async Task<DownloadResult> Download(string path, IProgress<int> IP)
        {
            var errors = new List<string>();
            var files = Info.Files;
            files.Reverse();
            Trace.WriteLine($"Download started: {files.Count} files");
            var tasks = files.Select(async (F) =>
            {
                try
                {
                    await DownloadFile(path, F);
                    Interlocked.Increment(ref Count);
                    IP.Report(Count);
                }
                catch (Exception e)
                {
                    var error = $"File: {F.Name}\nException: {e.Message}";
                    errors.Add(error);
                }
            });
            await Task.WhenAll(tasks);
            IP.Report(Count);
            Trace.WriteLine($"Download done: {files.Count} files");
            // TODO Add hash check
            return new DownloadResult(path, errors);
        }

        protected async Task<string> DownloadFile(string path, ModpackFile file)
        {
            await Semaphore.WaitAsync();
            var localFile = new FileInfo(Path.Combine(path, file.Path, file.Name));
            try
            {
                var URL = file.Url;
                if (string.IsNullOrEmpty(URL))
                {
                    var curseFile = (await CF.GetModFile(file.CurseForge.Project, file.CurseForge.File)).Data;
                    URL = curseFile.DownloadURL;
                    //IDK Why url is null; Controlled by mod author?
                    if (URL is null)
                    {
                        // It just works
                        var ID = file.CurseForge.File.ToString();
                        URL = $"{CDNEndpoint}/{ID[..4]}/{ID[4..]}/{curseFile.FileName}";
                    }
                }

                Trace.WriteLine($"Downloading file: {URL} ");
                using var responce = await Client.GetAsync(URL);
                responce.EnsureSuccessStatusCode();
                Directory.CreateDirectory(localFile.DirectoryName);
                using var FS = new FileStream(localFile.FullName, FileMode.Create);
                await responce.Content.CopyToAsync(FS);
            }
            finally
            {
                Semaphore.Release();
            }
            return localFile.FullName;
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