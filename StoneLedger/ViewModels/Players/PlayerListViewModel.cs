using PlayerDomain.Model;
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

        public ICommand LoadPlayersCommand { get; }
        public ICommand AddPlayerCommand { get; }

        public PlayerListViewModel(PlayerService playerService)
        {
            _playerService = playerService;

            LoadPlayersCommand = new Command(async () => await LoadPlayers());
            AddPlayerCommand = new Command(async () =>
                await Shell.Current.GoToAsync("playerdetail"));
        }

        private async Task LoadPlayers()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                PlayerCollection.Clear();
                var players = await _playerService.GetAllPlayersAsync();

                foreach (var p in players)
                    PlayerCollection.Add(p);
            }
            catch (Exception ex)
            {
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
