using System.Collections;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using NyaapiDotnet.Models;
using NyaapiDotnet.Service;

namespace NyaapiDotnet.UnitTests.Si
{
    public class NyaapiDotnet_SearchSiTest
    {
        [Fact]
        public async Task SearchRandom_ReturnResults()
        {
            var nyaapiService = new NyaapiService();
            ArrayList torrents = new();
            await foreach (Torrent t in nyaapiService.SearchTorrents())
            {
                torrents.Add(t);
            }
            Assert.NotEmpty(torrents);
        }

        [Fact]
        public async Task SearchForErai_ReturnEraiSubs()
        {
            var nyaapiService = new NyaapiService();
            await foreach (Torrent t in nyaapiService.SearchTorrents(Fansubs.EraiRaws, limit: 10))
            {
                Assert.Contains("[Erai-raws]", t.Name);
            }
        }
        
        [Fact]
        public async Task SearchForFanSubAndQuality_ReturnsCorrectInstances()
        {
            var nyaapiService = new NyaapiService();
            await foreach(Torrent t in nyaapiService.SearchTorrents(quality: Quality.FHD, fansubs: Fansubs.EraiRaws))
            {
                Assert.Contains("[Erai-raws]", t.Name);
                Assert.Contains("[1080p]", t.Name);
            }
        }
    }
}