using StoneLedger.Models;
using StoneLedger.Services.Api;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Players
{
    public class PlayerListViewModel : BaseViewModel
    {
        private readonly PlayerService _playerService;
        public ObservableCollection<Player> PlayerCollection { get; } = new();

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                if (SetProperty(ref _selectedPlayer, value) && value != null)
                {
                    OnPlayerSelected(value);
                }
            }
        }

        public ICommand AddPlayerCommand { get; }

        public PlayerListViewModel()
        {
            _playerService = new PlayerService();

            AddPlayerCommand = new Command(async () =>
                await Shell.Current.GoToAsync("playerdetail"));

            // Placeholder data so the page compiles and displays something
            LoadPlayers();
        }

        private async void LoadPlayers()
        {
            try 
            { 
                IsBusy = true; 
                var players = await _playerService.GetAllPlayersAsync();
                PlayerCollection.Clear(); 
                foreach (var p in players)
                    PlayerCollection.Add(p); 
            }
            catch (Exception ex)
            { 
                // Temporary debugging feedback
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            } 
            finally
            { 
                IsBusy = false; 
            }
        }

        private async void OnPlayerSelected(Player player)
        {
            if (player == null)
                return;

            await Shell.Current.GoToAsync($"playerdetail?id={player.Id}");
        }
    }
}
