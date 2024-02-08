using System.Collections;
using System.Collections.Generic;
using NyaapiDotnet.Models;
using NyaapiDotnet.Interfaces;
using NyaapiDotnet.Si.Models;

namespace NyaapiDotnet.Si
{
    public class SiClient : INyaaClient
    {

        private readonly Scraper scraper;

        public SiClient()
        {
            scraper = new Scraper();
        }

        public async IAsyncEnumerable<Torrent> SearchTorrents(Fansubs fansubs, Quality quality, string search, int limit, int page)
        {
            var queryParams = CreateParams(fansubs, quality, search, limit, page);
            await foreach (Torrent t in scraper.ScrapeTorrent(queryParams))
            {
                yield return t;
            }
        }

        private static SiRequestParams CreateParams(Fansubs fansubs = Fansubs.None, Quality quality = Quality.None, string search = "", int limit = 0, int page = 1)
        {
            var categories = new ArrayList
            {
                "1_2"
            };
            var queryParams = new SiRequestParams
            {
                Filters = "0",
                Query = CreateSearchQuery(fansubs, quality, search),
                Categories = categories,
                Limit = limit,
                Page = page,
                Sort = "id",
                Order = "desc"
            };

            return queryParams;
        }

        private static string CreateSearchQuery(Fansubs fansubs = Fansubs.None, Quality quality = Quality.None, string search = "")
        {
            string fansub = fansubs switch
            {
                Fansubs.EraiRaws => "[Erai-raws] ",
                Fansubs.Puya => "[PuyaSubs!] ",
                Fansubs.Fuyu => "[Fuyu] ",
                Fansubs.SSA => "[SSA] ",
                Fansubs.Durandal => "[DurandalSubs] ",
                _ => ""
            };
            string qual = quality switch
            {
                Quality.SD => " [480p]",
                Quality.HD => " [720p]",
                Quality.FHD => " [1080p]",
                _ => ""
            };

            return string.Concat(fansub, search, qual);
        }

    }
}