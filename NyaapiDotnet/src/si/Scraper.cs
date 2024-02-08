using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using NyaapiDotnet.Models;
using NyaapiDotnet.Si.Models;
using System.Text.RegularExpressions;

namespace NyaapiDotnet.Si
{
    public partial class Scraper
    {
        public async IAsyncEnumerable<Torrent> ScrapeTorrent(SiRequestParams queryParams)
        {
            string queryUrl = queryParams.buildQueryParams();
            string url = $"{SiConstants.url}/?{queryUrl}";

            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);
            // This is where the HTTP request happens, returns <IDocument> that we can query later
            var document = await context.OpenAsync(url);
            var torrentRows = document.QuerySelectorAll("tr.default");
            foreach(var row in torrentRows)
            {
                MatchCollection regxMatches = Regex().Matches(GetNthTd(row, 3).QuerySelector("a:nth-child(2)").GetAttribute("href"));
                _ = long.TryParse(GetNthTd(row, 2).QuerySelector("a").GetAttribute("href").Replace("/view/", ""), out long id);
                DateTime dt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(GetNthTd(row, 5).GetAttribute("data-timestamp"))).DateTime;
                _ = int.TryParse(GetNthTd(row, 6).TextContent, out int seeders);
                _ = int.TryParse(GetNthTd(row, 7).TextContent, out int leechers);
                _ = int.TryParse(GetNthTd(row, 8).TextContent, out int completed);


                SiTorrent torrent = new()
                {
                    Id = id,
                    Name = GetNthTd(row, 2).QuerySelector("a").TextContent.Trim(),
                    Hash = regxMatches.First().Value,
                    Date = dt,
                    Filesize = GetNthTd(row, 4).TextContent,
                    Category = GetNthTd(row, 1).QuerySelector("a").GetAttribute("href").Replace("/?c=", "").Replace(@"/\d{1,2}$/", "0"),
                    SubCategory = GetNthTd(row, 1).QuerySelector("a").GetAttribute("href").Replace("/?c=", ""),
                    Magnet = GetNthTd(row, 3).QuerySelector("a:nth-child(2)").GetAttribute("href"),
                    Torrent = SiConstants.url + GetNthTd(row, 3).QuerySelector("a:nth-child(1)").GetAttribute("href"),
                    Status = row.GetAttribute("class"),
                    Seeders = seeders,
                    Leechers = leechers,
                    Completed = completed,
                    Description = ""
                };
                yield return new Torrent(torrent);
            }
        }

        private IElement GetNthTd(IElement input, int element)
        {
            return input.QuerySelector($"td:nth-child({element})");
        }

        [GeneratedRegex("btih:(\\w+)")]
        private static partial Regex Regex();
    }
}
