namespace ModpacksCH.API
{
    public class ModpackDownloader : IDisposable
    {
        private readonly HttpClient Client = new();

        public Task Download(DownloadInfo info) => Download(info, default);

        public async Task Download(DownloadInfo info, IProgress<(int Total, int Done)> P)
        {
            var Modpack = info.Modpack;
            var Version = info.Version;
            var ModpackPath = info.OutPath;

            var Files = Version.Files.Where(F => info.IsServer ? !F.ClientOnly : !F.ServerOnly);

            int Count = 0, Max = Files.Count();
            foreach (var PackFile in Files)
            {
                P.Report((Count, Max));
                var File = new FileInfo(Path.Combine(ModpackPath, PackFile.Path, PackFile.Name));
                Directory.CreateDirectory(File.DirectoryName);
                using var Response = await Client.GetAsync(PackFile.Url);
                using var FS = new FileStream(File.FullName, FileMode.Create);
                await Response.Content.CopyToAsync(FS);
                Count++;
            }
            P.Report((Count, Max));
            //TODO Add hash check
        }

        #region Dispose
        private bool disposedValue;

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                }
                disposedValue = true;
            }
        }

        #endregion Dispose
    }
}