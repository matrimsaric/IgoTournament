using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Players;

public partial class PlayerDetailPage : ContentPage
{
	public PlayerDetailPage(PlayerDetailViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        Loaded += async (_, _) => await vm.LoadPortraitAsync();
    }
}