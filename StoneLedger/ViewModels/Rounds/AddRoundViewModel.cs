using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using System.Windows.Input;

[QueryProperty(nameof(TournamentId), "TournamentId")]
public class AddRoundViewModel : BaseViewModel
{
    private readonly RoundService _roundService;

    public Guid _tournamentGuid { get; set; }

    public string TournamentId
    {
        set
        {
            Console.WriteLine($"TournamentId setter hit with: {value}");

            if (Guid.TryParse(value, out var parsed))
            {
                _tournamentGuid = parsed;   // <-- THIS WAS MISSING
                Console.WriteLine($"Parsed GUID = {parsed}");

            }
        }
    }

    public string RoundName { get; set; }
    public int RoundNumber { get; set; }
    public DateTime? RoundDate { get; set; }
    public string Notes { get; set; }

    public ICommand SaveCommand { get; }

    public AddRoundViewModel(RoundService roundService)
    {
        _roundService = roundService;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(RoundName))
            return;

        var round = new Round
        {
            Id = Guid.NewGuid(),
            Name = RoundName,
            TournamentId = _tournamentGuid,
            RoundNumber = RoundNumber,
            RoundDate = RoundDate,
            Notes = Notes
        };

        await _roundService.CreateRoundAsync(round);

        await Shell.Current.GoToAsync(".."); // navigate back
    }
}
