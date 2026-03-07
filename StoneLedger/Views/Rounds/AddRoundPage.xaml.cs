namespace StoneLedger.Views.Rounds;

public partial class AddRoundPage : ContentPage
{
    public AddRoundPage(AddRoundViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
