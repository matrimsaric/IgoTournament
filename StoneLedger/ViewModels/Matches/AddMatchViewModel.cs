using CompetitionDomain.Model;
using PlayerDomain.Model;
using PlayerDomain.Services;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayerService = StoneLedger.Services.Api.PlayerService;

namespace StoneLedger.ViewModels.Matches
{
    [QueryProperty(nameof(RoundId), "RoundId")]
    public class AddMatchViewModel : BaseViewModel
    {
        private readonly MatchService _matchService;
        private readonly PlayerService _playerService;

        public ObservableCollection<Player> Players { get; } = new();

        public Player SelectedPlayer1 { get; set; }
        public Player SelectedPlayer2 { get; set; }

        public int BoardNumber { get; set; }
        public string BlackPlayerId { get; set; }
        public string WhitePlayerId { get; set; }
        public string Result { get; set; }
        public string SgfRaw { get; set; }

        public string SgfFilePath { get; set; }


        public ICommand SaveCommand { get; }
        public ICommand PickSgfCommand { get; }

        public Guid _roundId { get; set; }

        public string RoundId
        {
            set
            {
                Console.WriteLine($"TournamentId setter hit with: {value}");

                if (Guid.TryParse(value, out var parsed))
                {
                    Console.WriteLine($"Parsed Round = {parsed}");
                    _roundId = parsed;   // <-- THIS WAS MISSING
                }
            }
        }

        public AddMatchViewModel(MatchService matchService, PlayerService playerService)
        {
            _matchService = matchService;
            _playerService = playerService;
            SaveCommand = new Command(async () => await SaveAsync());
            PickSgfCommand = new Command(async () => await PickSgfAsync());

            _ = LoadPlayersAsync();
        }

        private async Task LoadPlayersAsync()
        {
            var list = await _playerService.GetAllPlayersAsync();

            Players.Clear();
            foreach (var p in list)
                Players.Add(p);
        }

        private async Task SaveAsync()
        {
            var match = new Match
            {
                Id = Guid.NewGuid(),
                RoundId = _roundId,
                BoardNumber = BoardNumber,
                BlackPlayerId = SelectedPlayer1.Id,
                WhitePlayerId = SelectedPlayer2.Id,
                Result = Result,
                GameDate = DateTime.Today,
                Name = "TEMP NAME" // will be auto‑generated later
            };

            //await _matchService.CreateMatchAsync(match);

            // Optionally, you could also save the SGF data to a separate service or associate it with the match here.

            await Shell.Current.GoToAsync("..");
        }

        private async Task PickSgfAsync()
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select SGF File",
                FileTypes = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                { DevicePlatform.WinUI, new[] { ".sgf" } },
                { DevicePlatform.Android, new[] { "application/octet-stream", "text/plain" } },
                { DevicePlatform.iOS, new[] { "public.data", "public.text" } },
                { DevicePlatform.MacCatalyst, new[] { "public.data", "public.text" } }
                    })
            });

            if (result != null)
            {
                // Read the SGF file as text
                SgfRaw = await File.ReadAllTextAsync(result.FullPath);

                // Optional: show the filename in the UI
                SgfFilePath = result.FileName;

                OnPropertyChanged(nameof(SgfFilePath));
            }
        }
    }
}
