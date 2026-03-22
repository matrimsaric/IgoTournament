using StoneLedger.Models;
using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

public partial class ExpandedSgfPage : ContentPage
{
    public ExpandedSgfPage(IList<SgfMove> moves)
    {
        InitializeComponent();
        BindingContext = new ExpandedSgfViewModel(moves);
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
