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
using System.Threading;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Binding;
using System.Reactive;

namespace NyaaGui.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ObservableCollection<EpisodeViewModel> _torrents = [];
        public ICommand RefreshList { get; }
        public ObservableCollection<EpisodeViewModel> Torrents {  get { return _torrents; } }
        [ObservableProperty]
        private EpisodeViewModel? _selectedEpisode;
        private CancellationTokenSource? _cancellationTokenSource;
        public Interaction<EpisodeDetailsWindowViewModel, bool?> ShowDetails { get; }
        public ICommand ShowDetailsCommand { get; }

        [ObservableProperty]
        private string? _searchText;
        [ObservableProperty]
        private bool _isBusy;

        public MainWindowViewModel() {
            ShowDetails = new Interaction<EpisodeDetailsWindowViewModel, bool?>();
            ShowDetailsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var details = new EpisodeDetailsWindowViewModel();
                var result = await ShowDetails.Handle(details);
            });
            
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
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            IsBusy = true;
            _torrents?.Clear();

            if (!string.IsNullOrEmpty(s))
            {
                await foreach (var episode in Episode.SearchAsync(s, cancellationToken))
                {
                    var vm = new EpisodeViewModel(episode);
                    _torrents?.Add(vm);
                }
            }

            IsBusy = false;
        }

        partial void OnSelectedEpisodeChanged(EpisodeViewModel? value)
        {
            if (value != null)
            {
               ShowDetailsCommand.Execute(null);
            }
        }
    }
}