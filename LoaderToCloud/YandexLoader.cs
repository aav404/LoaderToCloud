using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoaderToCloud
{
    class YandexLoader
    {
        public List<string> FilePaths { get; set; }

        const string baseAdress = @"https://cloud-api.yandex.net";

        private string token;

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = $"OAuth {value}";
            }
        }

        public async Task LoadFilesAsync()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAdress)
            };

            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            await CreateDirectoryAsync(httpClient);

            Load(httpClient);
        }

        private void Load(HttpClient httpClient)
        {
            Task.WaitAll(FilePaths.Select((s, i) => Task.Run(async () =>
            {
                var responseJson = await httpClient.GetStringAsync($"/v1/disk/resources/upload?path=Directory/FileName{i}.pdf");

                var responseObj = JsonConvert.DeserializeObject<Response>(responseJson);

                var file = File.ReadAllBytes(s);

                var content = new ByteArrayContent(file);

                await httpClient.PutAsync(responseObj.Href, content);
            })).ToArray());
        }

        private async Task CreateDirectoryAsync(HttpClient httpClient)
        {
            await httpClient.PutAsync($"/v1/disk/resources?path=Directory", new StringContent(string.Empty));
        }

        private class Response
        {
            [JsonProperty("href")]
            public string Href { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("templated")]
            public bool Templated { get; set; }
        }
    }
}
