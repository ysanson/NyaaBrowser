using DynamicData;
using NyaaGui.ViewModels;
using ReactiveUI;
using System.Windows.Input;
using NyaapiDotnet.Models;
using System.Collections.Generic;
using NyaapiDotnet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace NyaaGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Torrent> _torrents;
        private readonly NyaapiService _service;
        public ICommand RefreshList { get; }

        public ObservableCollection<Torrent> Torrents {  get { return _torrents; } }

        public MainWindowViewModel() { 
            _service = new NyaapiService();
            _torrents = [];
            RefreshList = ReactiveCommand.Create(LoadData);
        }

        

        private async Task LoadData()
        {
           var torrents = _service.SearchTorrents();
           
           _torrents.AddRange(await torrents.ToListAsync());
        }
    }
}