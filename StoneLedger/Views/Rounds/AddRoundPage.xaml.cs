using CompetitionDomain.Model;
using StoneLedger.ViewModels.Rounds;

namespace StoneLedger.Views.Rounds;

public partial class AddRoundPage : ContentPage
{
   
    public AddRoundPage(AddRoundViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public Guid _tournamentGuid { get; set; }

    public string TournamentId
    {
        set
        {
            Console.WriteLine($"TournamentId setter hit with: {value}");

            if (Guid.TryParse(value, out var parsed))
            {
                Console.WriteLine($"Parsed GUID = {parsed}");

                if (BindingContext is RoundListViewModel vm)
                    _ = vm.LoadRoundsAsync(parsed); // fire and forget
            }
        }
    }
}
