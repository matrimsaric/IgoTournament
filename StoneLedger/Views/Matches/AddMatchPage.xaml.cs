using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

public partial class AddMatchPage : ContentPage
{
    public AddMatchPage(AddMatchViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
