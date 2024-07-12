using Avalonia.Controls;
using Avalonia.ReactiveUI;
using NyaaGui.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace NyaaGui.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{

    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action =>
            action(ViewModel!.ShowDetails.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<EpisodeDetailsWindowViewModel,bool?> interaction)
    {
        var dialog = new EpisodeDetailsWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<bool?>(this);
        interaction.SetOutput(result);
    }
}