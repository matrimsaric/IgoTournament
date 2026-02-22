using StoneLedger.ViewModels;
using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}