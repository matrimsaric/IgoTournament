using StoneLedger.ViewModels.Rounds;

namespace StoneLedger.Views.Rounds;

public partial class RoundListPage : ContentPage
{
   

    public RoundListPage(RoundListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}
