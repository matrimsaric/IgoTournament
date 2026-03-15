using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(RoundId), "RoundId")]
[QueryProperty(nameof(RoundNumber), "RoundNumber")]
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
                {
                    _ = vm.RoundId = parsed;
                    _ = vm.LoadMatchesAsync(parsed);
                }
                    
            }
        }
    }

    public int _roundNumber;
    public int RoundNumber
    {
        set
        {
            if (BindingContext is MatchListViewModel vm)
                _ = vm.RoundNumber = value;
        }
    }
}
