using System.Collections;
using System;
using System.Collections.Generic;
using NyaapiDotnet.Models;
using NyaapiDotnet.Interfaces;
using NyaapiDotnet.Si;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NyaapiDotnet.Service
{
    public class NyaapiService : INyaapiService
    {
        private readonly SiClient siClient;

        public NyaapiService()
        {
            siClient = new SiClient();
        }

        public async IAsyncEnumerable<Torrent> SearchTorrents(Fansubs fansubs = Fansubs.None, Quality quality = Quality.None, string search = "", int limit = 0, int page = 1, [EnumeratorCancellation] CancellationToken token = default)
        {
            await foreach (Torrent t in siClient.SearchTorrents(fansubs, quality, search, limit, page, token))
            {
                yield return t;
            }
            
        }
    }
}