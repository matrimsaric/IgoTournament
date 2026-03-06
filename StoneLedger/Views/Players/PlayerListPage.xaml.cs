using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Players;

public partial class PlayerListPage : ContentPage
{
    public PlayerListPage(PlayerListViewModel vm)
    {
        InitializeComponent();
        Console.WriteLine("PlayerListPage constructed via DI");
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("OnAppearing fired");

        if (BindingContext is PlayerListViewModel vm)
        {
            Console.WriteLine("BindingContext is PlayerListViewModel");
            vm.LoadPlayersCommand.Execute(null);
        }
        else
        {
            Console.WriteLine("BindingContext is NOT PlayerListViewModel");
        }
    }
}