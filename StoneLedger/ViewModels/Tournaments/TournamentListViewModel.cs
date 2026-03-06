using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StoneLedger.Services.ViewModels.Tournaments
{
    public class TournamentListViewModel : BaseViewModel
    {
        private readonly TournamentService _service;

        public ObservableCollection<Tournament> Tournaments { get; }
            = new ObservableCollection<Tournament>();

        public ICommand LoadTournamentsCommand { get; }
        public ICommand SelectTournamentCommand { get; }

        public TournamentListViewModel(TournamentService service)
        {
            _service = service;

            LoadTournamentsCommand = new Command(async () => await LoadTournaments());
            SelectTournamentCommand = new Command<Tournament>(OnTournamentSelected);
        }

        private async Task LoadTournaments()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Tournaments.Clear();
                var items = await _service.GetAllTournamentsAsync();

                foreach (var t in items)
                    Tournaments.Add(t);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnTournamentSelected(Tournament tournament)
        {
            if (tournament == null)
                return;

            // Navigate to Rounds page (placeholder for now)
            await Shell.Current.GoToAsync(
                $"rounds?tournamentId={tournament.Id}");
        }
    }
}
