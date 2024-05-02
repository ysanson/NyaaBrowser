using NyaaGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static AnitomySharp.AnitomySharp;
using System.Text;
using System.Threading.Tasks;

namespace NyaaGui.ViewModels
{
    public class EpisodeViewModel : ViewModelBase
    {
        private readonly Episode _episode;
        private readonly IEnumerable<AnitomySharp.Element>? _elements;

        public string Title => _episode.Name;
        public string Magnet => _episode.Magnet;
        public string AnimeCover => _episode.CoverUrl;
        public string Name
        {
            get
            {
                if (_elements is null) return "";
                var titleElem = _elements.SingleOrDefault(x => x.Category == AnitomySharp.Element.ElementCategory.ElementAnimeTitle);
                if (titleElem == null) return "";
                else return titleElem.Value;
            }
        }

        public string EpisodeNumber
        {
            get
            {
                if (_elements is null) return "0";
                var elem = _elements.SingleOrDefault(x => x.Category == AnitomySharp.Element.ElementCategory.ElementEpisodeNumber);
                if (elem == null) return "0";
                else return elem.Value;
            }
        }

        public EpisodeViewModel()
        {
            _episode = new Episode(0, "", DateTime.Now, "", "", "", "", "");
        }

        public EpisodeViewModel(Episode episode)
        {
            _episode = episode;
            _elements = Parse(episode.Name);
        }
    }
}
