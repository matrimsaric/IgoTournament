using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Players;

public partial class PlayerListPage : ContentPage
{
	public PlayerListPage()
	{
		InitializeComponent();
        BindingContext = new PlayerListViewModel();
    }
}