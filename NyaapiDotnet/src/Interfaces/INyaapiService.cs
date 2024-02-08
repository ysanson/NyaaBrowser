using NyaapiDotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyaapiDotnet.Interfaces
{
    public interface INyaapiService
    {
        IAsyncEnumerable<Torrent> SearchTorrents(Fansubs fansubs = Fansubs.None, Quality quality = Quality.None, string search = "", int limit = 0, int page = 1);
    }
}
