using MyAnimeApi.src;
using MyAnimeApi.src.models;
using NyaapiDotnet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NyaaGui.Models
{
    public class Episode(long id, string name, DateTime date, string category, string subCategory, string magnet, string torrentAddress, string coverUrl)
    {
        private static readonly NyaapiService _service = new();
        private static readonly MalClient _malClient = new();

        public long Id { get; set; } = id;
        public string Name { get; set; } = name;
        public DateTime Date { get; set; } = date;
        public string Category { get; set; } = category;
        public string SubCategory { get; set; } = subCategory;
        public string Magnet { get; set; } = magnet;
        public string TorrentAddress { get; set; } = torrentAddress;
        public string CoverUrl { get; set; } = coverUrl;

        public static async IAsyncEnumerable<Episode> SearchAsync(string searchTerm, [EnumeratorCancellation] CancellationToken token)
        {
            var torrents = _service.SearchTorrents(search: searchTerm, token: token);
            await foreach (var x in torrents)
            {
                AnimeInfo? myAnimeList = await _malClient.GetAnimeInfoByName(x.CleanName, token);
                if (myAnimeList is null)
                {
                    yield return new Episode(x.Id, x.Name, x.Date, x.Category, x.SubCategory, x.Magnet, x.TorrentAddress, "");

                }
                else
                {
                    yield return new Episode(x.Id, x.Name, x.Date, x.Category, x.SubCategory, x.Magnet, x.TorrentAddress, myAnimeList.MediumPicture);
                }
            }
        }

        public static async IAsyncEnumerable<Episode> InitDataAsync([EnumeratorCancellation] CancellationToken token)
        {
            var torrents = _service.SearchTorrents(token: token);
            await foreach (var x in torrents)
            {
                string? imageUrl = await _malClient.GetPictureByAnimeName(x.CleanName, token);
                if (imageUrl is null)
                {
                    yield return new Episode(x.Id, x.Name, x.Date, x.Category, x.SubCategory, x.Magnet, x.TorrentAddress, "");

                }
                else
                {
                    yield return new Episode(x.Id, x.Name, x.Date, x.Category, x.SubCategory, x.Magnet, x.TorrentAddress, imageUrl);
                }
            }
        }
    }
}
