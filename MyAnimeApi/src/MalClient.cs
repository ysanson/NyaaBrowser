using Microsoft.Extensions.Configuration;
using MyAnimeApi.src.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyAnimeApi.src
{
    internal class MalClient
    {
        private readonly HttpClient sharedClient;

        public MalClient() {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<MalClient>()
            .Build();
            sharedClient = new()
            {
                BaseAddress = new Uri("https://api.myanimelist.net/v2/anime/"),
            };
            sharedClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", config["ClientId"]);
            sharedClient.DefaultRequestHeaders.Accept.Clear();
            sharedClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AnimeInfo> GetAnimeinfo(int animeId)
        {
            await using Stream stream = await sharedClient.GetStreamAsync(animeId.ToString());
            var infos = await JsonSerializer.DeserializeAsync<AnimeInfo>(stream);
            if (infos != null)
            { 
                return infos;
            }
            else
            {
                throw new InfoNotFoundException();
            }
        }
    }
}
