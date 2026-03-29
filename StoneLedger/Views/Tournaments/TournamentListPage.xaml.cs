using CompetitionDomain.Model;
using StoneLedger.Views.Matches;
using StoneLedger.Views.Rounds;

namespace StoneLedger.Views.Tournaments
{
    public partial class TournamentListPage : ContentPage
    {
        // ((CollectionView)sender).SelectedItem = null;
        private readonly TournamentListViewModel _vm;
       

        public TournamentListPage(TournamentListViewModel vm)
        {
            InitializeComponent();
            BindingContext = _vm = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadTournamentsCommand.Execute(null);
        }

        private async void OnTournamentSelected(Tournament tournament)
        {
            await Shell.Current.GoToAsync($"{nameof(RoundListPage)}?TournamentId={tournament.Id}");

            _vm.SelectedTournament = null;
        }


    }
}
