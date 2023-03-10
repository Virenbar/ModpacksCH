using ModpacksCH.API.Model;

namespace ModpacksCH.API
{
    public class CHClient : BaseClient
    {
        private const string EndPoint = "https://api.modpacks.ch";

        public CHClient() : base(EndPoint) { }

        public Task<CFModpack> GetCFModpack(int modpackID) => Get<CFModpack>($"/public/curseforge/{modpackID}");

        public Task<CFVersion> GetCFVersion(int modpackID, int versionID) => Get<CFVersion>($"/public/curseforge/{modpackID}/{versionID}");

        public Task<CHModpack> GetCHModpack(int modpackID) => Get<CHModpack>($"/public/modpack/{modpackID}");

        public Task<CHVersion> GetCHVersion(int modpackID, int versionID) => Get<CHVersion>($"/public/modpack/{modpackID}/{versionID}");

        public async Task<ModpackManifest> GetModpack(int modpackID) => modpackID > 1000 ? await GetCFModpack(modpackID) : await GetCHModpack(modpackID);

        public async Task<VersionManifest> GetVersion(int modpackID, int versionID) => modpackID > 1000 ? await GetCFVersion(modpackID, versionID) : await GetCHVersion(modpackID, versionID);

        public Task<SearchResult> Search(string name) => Search(name, 10);

        public Task<SearchResult> Search(string name, int limit) => Get<SearchResult>($"/public/modpack/search/{limit}?term={name}");
    }
}