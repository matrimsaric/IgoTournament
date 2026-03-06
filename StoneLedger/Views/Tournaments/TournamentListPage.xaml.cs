using StoneLedger.Services.ViewModels.Tournaments;

namespace StoneLedger.Views.Tournaments
{
    public partial class TournamentListPage : ContentPage
    {
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
    }
}
