using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using StoneLedger.Views.Rounds;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class TournamentListViewModel : BaseViewModel
{
    private readonly TournamentService _tournamentService;

    public ObservableCollection<Tournament> Tournaments { get; }
        = new ObservableCollection<Tournament>();

    public ICommand LoadTournamentsCommand { get; }

    public TournamentListViewModel(TournamentService tournamentService)
    {
        _tournamentService = tournamentService;
        LoadTournamentsCommand = new Command(async () => await LoadTournaments());
    }

    private async Task LoadTournaments()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            Tournaments.Clear();
            var items = await _tournamentService.GetAllTournamentsAsync();

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

        await Shell.Current.GoToAsync($"{nameof(RoundListPage)}?TournamentId={tournament.Id}");

        SelectedTournament = null;
    }

    private Tournament? _selectedTournament;
    public Tournament? SelectedTournament
    {
        get => _selectedTournament;
        set
        {
            if (SetProperty(ref _selectedTournament, value))
            {
                if (value != null)
                    OnTournamentSelected(value);
            }
        }
    }
}
