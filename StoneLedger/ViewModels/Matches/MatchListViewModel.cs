using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using StoneLedger.Views.Matches;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Matches
{
    public class MatchListViewModel : BaseViewModel
    {
        private readonly IMatchService _matchService;
        public ICommand AddMatchCommand { get; }

        public ObservableCollection<Match> Matches { get; }
            = new ObservableCollection<Match>();

        private Guid _roundId;
        public Guid RoundId
        {
            get => _roundId;
            set
            {
                SetProperty(ref _roundId, value);
                _ = LoadMatchesAsync(value);
            }
        }

        private int _roundNumber;
        public int RoundNumber
        {
            get => _roundNumber;
            set
            {
                SetProperty(ref _roundNumber, value);
            }
        }

        public MatchListViewModel(MatchService matchService)
        {
            _matchService = matchService;
            AddMatchCommand = new Command(OnAddMatch);
        }

        public async Task LoadMatchesAsync(Guid roundId)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                _roundId = roundId;
                Matches.Clear();
                var items = await _matchService.GetMatchesForRoundAsync(roundId);

                foreach (var m in items)
                    Matches.Add(m);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Match? _selectedMatch;
        public Match? SelectedMatch
        {
            get => _selectedMatch;
            set
            {
                if (SetProperty(ref _selectedMatch, value))
                {
                    if (value != null)
                        OnMatchSelected(value);
                }
            }
        }

        private async void OnMatchSelected(Match match)
        {
            // Placeholder navigation for now
            // await Shell.Current.DisplayAlert("Match Selected", match.Name, "OK");

            // Later:
            await Shell.Current.GoToAsync($"{nameof(MatchDetailPage)}?MatchId={match.Id}");
            // await Shell.Current.GoToAsync($"//{nameof(MatchDetailPage)}?MatchId={match.Id}");

            SelectedMatch = null;
        }

        private async void OnAddMatch()
        {
            //await Shell.Current.GoToAsync($"//{nameof(AddMatchPage)}?RoundId={_roundId}&RoundNumber={RoundNumber}");
            await Shell.Current.GoToAsync($"{nameof(AddMatchPage)}?RoundId={_roundId}&RoundNumber={RoundNumber}");
        }
    }
}
