using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Players;

public partial class PlayerDetailPage : ContentPage
{
	public PlayerDetailPage()
	{
		InitializeComponent();
        BindingContext = new PlayerDetailViewModel();
    }
}