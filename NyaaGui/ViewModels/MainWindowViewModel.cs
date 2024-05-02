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
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System;
using NyaaGui.Models;

namespace NyaaGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableCollection<EpisodeViewModel> _torrents = [];
        private EpisodeViewModel? _selectedEpisode;
        public ICommand RefreshList { get; }

        public ObservableCollection<EpisodeViewModel> Torrents {  get { return _torrents; } }

        public EpisodeViewModel? SelectedEpisode { get => _selectedEpisode; set => this.RaiseAndSetIfChanged(ref _selectedEpisode, value); }

        private string? _searchText;
        private bool _isBusy;

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        public MainWindowViewModel() { 
            RefreshList = ReactiveCommand.Create(InitList);
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch!);
           RxApp.MainThreadScheduler.Schedule(InitList);
        }

        private async void InitList()
        {
            IsBusy = true;
            _torrents?.Clear();
            await foreach (var episode in Episode.InitDataAsync(default))
            {
                var vm = new EpisodeViewModel(episode);
                _torrents?.Add(vm);
            }
            IsBusy = false;
        }

        private async void DoSearch(string? s)
        {
            IsBusy = true;
            _torrents?.Clear();

            if (!string.IsNullOrEmpty(s))
            {
                await foreach (var episode in Episode.SearchAsync(s, default))
                {
                    var vm = new EpisodeViewModel(episode);
                    _torrents?.Add(vm);
                }
            }

            IsBusy = false;
        }
    }
}