using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace ModpacksCH.API
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient Client;

        static BaseClient()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        protected BaseClient(string EndPoint) : this() { Client.BaseAddress = new Uri(EndPoint); }

        protected BaseClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<T> Get<T>(string Resource)
        {
            using var Request = new HttpRequestMessage(HttpMethod.Get, Resource);
            using var Responce = await Client.SendAsync(Request).ConfigureAwait(false);
            Responce.EnsureSuccessStatusCode();
            return await Deserialize<T>(Responce).ConfigureAwait(false);
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage HRM)
        {
            var S = await HRM.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var SR = new StreamReader(S);
            string Response = SR.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(Response);
        }

        #region Dispose

        public void Dispose()
        {
            ((IDisposable)Client).Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}