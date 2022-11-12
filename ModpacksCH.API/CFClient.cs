using ModpacksCH.API.Model.CurseForge;

namespace ModpacksCH.API
{
    public class CFClient : BaseClient
    {
        private const string EndPoint = "https://api.curseforge.com";
        private readonly string Key = "JDJhJDEwJENrUUkyMHNRcy9KTHlMLmYxWi4uLy51dENObk5sbVJnUjZjZkNpTENlQ2h1dS8vY3ZIT1Fx".Decode(); // OwO What's this

        public CFClient() : base(EndPoint)
        {
            Client.DefaultRequestHeaders.Add("x-api-key", Key);
        }

        public Task<ModFileResponse> GetModFile(int modID, int fileID) => Get<ModFileResponse>($"/v1/mods/{modID}/files/{fileID}");
    }
}