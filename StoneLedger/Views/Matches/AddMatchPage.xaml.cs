using CompetitionDomain.Model;
using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(RoundId), "RoundId")]
[QueryProperty(nameof(RoundNumber), "RoundNumber")]

public partial class AddMatchPage : ContentPage
{
    public AddMatchPage(AddMatchViewModel vm)
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
                if (BindingContext is AddMatchViewModel vm)
                    _ = vm.RoundId = value;
            }
        }
    }

    public int _roundNumber;
    public int RoundNumber
    {
        set
        {
            _roundNumber = value;
            if (BindingContext is AddMatchViewModel vm)
                _ = vm.RoundNumber = value;
        }
    }
}
