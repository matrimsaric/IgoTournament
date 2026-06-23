using StoneLedger.ViewModels.JosekiStudy;

namespace StoneLedger.Views.JosekiStudy;

public partial class JosekiListPage : ContentPage
{
	public JosekiListPage(JosekiListViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}