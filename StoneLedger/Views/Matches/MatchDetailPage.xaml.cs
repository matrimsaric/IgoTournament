using CompetitionDomain.Model;
using StoneLedger.ViewModels.Matches;
using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(MatchId), "MatchId")]
public partial class MatchDetailPage : ContentPage
{


    private readonly MatchDetailViewModel _vm;

    public MatchDetailPage(MatchDetailViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;

      
    }

    public string MatchId
    {
        set
        {
            Console.WriteLine($"MatchDetailPage received MatchId = {value}");

            if (Guid.TryParse(value, out var parsed))
            {
                if (BindingContext is MatchDetailViewModel vm)
                   _ =  _vm.LoadMatchAsync(parsed);
            }
        }
    }
}