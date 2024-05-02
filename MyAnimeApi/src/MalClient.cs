using Microsoft.Extensions.Configuration;
using MyAnimeApi.src.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

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
                BaseAddress = new Uri("https://api.myanimelist.net/v2/"),
            };
            sharedClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", config["ClientId"]);
            sharedClient.DefaultRequestHeaders.Accept.Clear();
            sharedClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AnimeInfo> GetAnimeinfo(int animeId, CancellationToken token = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"fields", "id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics'"}
            };
            var dictEncoded = new FormUrlEncodedContent(queryParams);
            var urlEncoded = await dictEncoded.ReadAsStringAsync(token);
            await using Stream stream = await sharedClient.GetStreamAsync($"anime/${animeId}?${urlEncoded}", token);
            var infos = await JsonSerializer.DeserializeAsync<AnimeInfo>(stream, cancellationToken: token);
            if (infos is not null)
            { 
                return infos;
            }
            else
            {
                throw new InfoNotFoundException();
            }
        }

        public async Task<List<AnimeNode>> SearchAnime(string searchTerm, CancellationToken token = default)
        {
            var results = await sharedClient.GetFromJsonAsync<AnimeSearchResult>($"anime?q={searchTerm}", token);
            if (results is not null)
            { 
                return results.Results;
            }
            else
            {
                return [];
            }
        }

        public async Task<AnimeInfo?> GetAnimeInfoByName(string name, CancellationToken token = default)
        {
            var bestResult = (await SearchAnime(name, token)).First();
            if (bestResult is not null)
            {
                return await GetAnimeinfo(bestResult.Node.Id, token);
            } else
            {
                return null;
            }
        }

        public async Task<string?> GetPictureByAnimeName(string name, CancellationToken token = default)
        {
            try
            {
                var bestResult = (await SearchAnime(name, token)).First();
                if (bestResult is not null)
                {
                    return bestResult.Node.Pictures.Medium;
                }
                else
                {
                    return null;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        } 
    }
}
