using NyaapiDotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NyaapiDotnet.Interfaces
{
    public interface INyaaClient
    {
        IAsyncEnumerable<Torrent> SearchTorrents(Fansubs fansubs, Quality quality, string search, int limit, int page, CancellationToken token);
    }
}
