using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(RoundId), "RoundId")]
public partial class MatchListPage : ContentPage
{
    public MatchListPage(MatchListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public string RoundId
    {
        set
        {
            Console.WriteLine($"MatchListPage received RoundId = {value}");

            if (Guid.TryParse(value, out var parsed))
            {
                if (BindingContext is MatchListViewModel vm)
                    _ = vm.LoadMatchesAsync(parsed);
            }
        }
    }
}
