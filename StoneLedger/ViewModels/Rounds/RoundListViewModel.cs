using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using StoneLedger.Views.Matches;
using StoneLedger.Views.Rounds;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Rounds
{
    [QueryProperty(nameof(TournamentId), "TournamentId")]
    public class RoundListViewModel : BaseViewModel
    {
        public Guid _tournamentGuid { get; set; }

        public string TournamentId
        {
            set
            {
                Console.WriteLine($"TournamentId setter hit with: {value}");

                if (Guid.TryParse(value, out var parsed))
                {
                    Console.WriteLine($"Parsed GUID = {parsed}");
                    _tournamentGuid = parsed;   // <-- THIS WAS MISSING

                    _ = LoadRoundsAsync(parsed); // fire and forget
                }
            }
        }
        private readonly IRoundService _roundService;
        public ICommand AddRoundCommand { get; }

        public ObservableCollection<Round> Rounds { get; }
            = new ObservableCollection<Round>();

        public RoundListViewModel(RoundService roundService)
        {
            _roundService = roundService;
            AddRoundCommand = new Command(OnAddRound);
        }

        public async Task LoadRoundsAsync(Guid tournamentId)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Rounds.Clear();
                var items = await _roundService.GetRoundsForTournamentAsync(tournamentId);

                foreach (var r in items)
                    Rounds.Add(r);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Round _selectedRound;
        public Round SelectedRound
        {
            get => _selectedRound;
            set
            {
                if (SetProperty(ref _selectedRound, value))
                {
                    if (value != null)
                        OnRoundSelected(value);
                }
            }
        }

        private async void OnRoundSelected(Round round)
        {
            // TODO: Navigate to MatchListPage
           // await Shell.Current.DisplayAlert("Round Selected", round.Name, "OK");

            await Shell.Current.GoToAsync(
        $"///{nameof(MatchListPage)}?RoundId={round.Id}"
    );

            SelectedRound = null;
        }

        private async void OnAddRound()
        {
            await Shell.Current.GoToAsync($"///{nameof(AddRoundPage)}?TournamentId={_tournamentGuid}");

        }

    }
}
