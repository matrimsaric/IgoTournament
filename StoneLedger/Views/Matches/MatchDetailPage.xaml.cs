using CompetitionDomain.Model;
using StoneLedger.ViewModels.Matches;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(MatchId), "MatchId")]
public partial class MatchDetailPage : ContentPage
{
    

    public MatchDetailPage()
	{
		InitializeComponent();
	}

    public string MatchId
    {
        set
        {
            Console.WriteLine($"MatchDetailPage received MatchId = {value}");

            if (Guid.TryParse(value, out var parsed))
            {
              //  if (BindingContext is AddMatchViewModel vm)
                   // _ = vm.RoundId = value;
            }
        }
    }
}