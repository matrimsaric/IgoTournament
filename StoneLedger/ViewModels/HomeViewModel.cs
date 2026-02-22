using System.Windows.Input;

namespace StoneLedger.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand GoToPlayersCommand { get; }
        public ICommand GoToMatchesCommand { get; }
        public ICommand GoToRoundsCommand { get; }
        public ICommand GoToReplayCommand { get; }

        public HomeViewModel()
        {
            GoToPlayersCommand = new Command(async () =>
                await Shell.Current.GoToAsync("players"));

            GoToMatchesCommand = new Command(async () =>
                await Shell.Current.GoToAsync("matches"));

            GoToRoundsCommand = new Command(async () =>
                await Shell.Current.GoToAsync("rounds"));

            GoToReplayCommand = new Command(async () =>
                await Shell.Current.GoToAsync("replay"));
        }
    }
}
