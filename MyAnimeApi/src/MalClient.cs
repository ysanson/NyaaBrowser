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
    public class MalClient
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
            var queryParams = new Dictionary<string, string>
            {
                {"fields", "id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics'"}
            };
            var dictEncoded = new FormUrlEncodedContent(queryParams);
            var urlEncoded = await dictEncoded.ReadAsStringAsync();
            await using Stream stream = await sharedClient.GetStreamAsync($"${animeId}?${urlEncoded}");
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

        public async Task<List<AnimeSearch>> SearchAnime(string searchTerm)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"q", searchTerm}
            };
            var dictEncoded = new FormUrlEncodedContent(queryParams);
            var urlEncoded = await dictEncoded.ReadAsStringAsync();
            await using Stream stream = await sharedClient.GetStreamAsync($"?${urlEncoded}");
            return (await JsonSerializer.DeserializeAsync<AnimeSearchResult>(stream))?.Results ?? [];
        }

        public async Task<AnimeInfo?> GetAnimeInfoByName(string name)
        {
            var bestResult = (await SearchAnime(name)).First();
            if (bestResult != null)
            {
                return await GetAnimeinfo(bestResult.Id);
            } else
            {
                return null;
            }
        }
    }
}
