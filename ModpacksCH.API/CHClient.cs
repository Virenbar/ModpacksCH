using ModpacksCH.API.Model;

namespace ModpacksCH
{
    public class CHClient : BaseClient
    {
        private const string EndPoint = "https://api.modpacks.ch";

        public CHClient() : base(EndPoint) { }

        public Task<ModpackManifest> GetCFModpack(int modpackID) => Get<ModpackManifest>($"/public/curseforge/{modpackID}");

        public Task<VersionManifest> GetCFVersion(int modpackID, int versionID) => Get<VersionManifest>($"/public/curseforge/{modpackID}/{versionID}");

        public Task<ModpackManifest> GetCHModpack(int modpackID) => Get<ModpackManifest>($"/public/modpack/{modpackID}");

        public Task<VersionManifest> GetCHVersion(int modpackID, int versionID) => Get<VersionManifest>($"/public/modpack/{modpackID}/{versionID}");

        public Task<ModpackManifest> GetModpack(int modpackID) => modpackID > 1000 ? GetCFModpack(modpackID) : GetCHModpack(modpackID);

        public Task<VersionManifest> GetVersion(int modpackID, int versionID) => modpackID > 1000 ? GetCFVersion(modpackID, versionID) : GetCHVersion(modpackID, versionID);

        public Task<SearchResult> Search(string name) => Search(name, 10);

        public Task<SearchResult> Search(string name, int limit) => Get<SearchResult>($"/public/modpack/search/{limit}?term={name}");
    }
}