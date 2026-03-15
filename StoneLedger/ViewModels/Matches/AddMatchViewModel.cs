using CompetitionDomain.Model;
using PlayerDomain.Model;
using PlayerDomain.Services;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using StoneLedger.Views.Matches;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayerService = StoneLedger.Services.Api.PlayerService;

namespace StoneLedger.ViewModels.Matches
{
    [QueryProperty(nameof(RoundId), "RoundId")]
    [QueryProperty(nameof(RoundNumber), "RoundNumber")]
    public class AddMatchViewModel : BaseViewModel
    {
        private readonly MatchService _matchService;
        private readonly SgfService _sgfService;
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

        public string  RoundId
        {
            set
            {
                if (Guid.TryParse(value, out var parsed))
                {
                    Console.WriteLine($"Parsed GUID = {parsed}");
                    _roundId = parsed;   // <-- THIS WAS MISSING

                    _ = LoadPlayersAsync(); // fire and forget
                }
            }
        }

        public int _roundNumber { get; set; }
        public int RoundNumber
        {
            set { 
                _roundNumber = value;
                    Console.WriteLine($"RoundNumber setter hit with: {value}");
            }
        }

        public List<string> WinnerOptions { get; } = new() { "Black", "White", "Draw" };
        public string SelectedWinner { get; set; }

        public List<string> ResultTypes { get; } = new() { "Resign", "Timeout", "Other", "Points" };
        private string _selectedResultType;
        public string SelectedResultType
        {
            get => _selectedResultType;
            set
            {
                if (_selectedResultType != value)
                {
                    _selectedResultType = value;
                    OnPropertyChanged(); // updates SelectedResultType binding
                    OnPropertyChanged(nameof(IsPointsVisible)); // updates visibility
                }
            }
        }

        public string Points { get; set; } // string so binding is easy

        public bool IsPointsVisible => SelectedResultType == "Points";



        public AddMatchViewModel(MatchService matchService, PlayerService playerService, SgfService sgfService)
        {
            _matchService = matchService;
            _playerService = playerService;
            _sgfService = sgfService;
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
                Name = $"{SelectedPlayer1.Name} vs {SelectedPlayer2.Name}",
                RoundId = _roundId,
                BoardNumber = BoardNumber,
                BlackPlayerId = SelectedPlayer1.Id,
                WhitePlayerId = SelectedPlayer2.Id,
                WinnerId = SelectedPlayer1.Id,
                Result = string.IsNullOrEmpty(Result) ? BuildResult() : Result,
                GameDate = DateTime.Today.Date
            };

            var sgfGenerated = await BuildSgfRecord(match);

            match.SgfId= sgfGenerated.Id;
            await _matchService.CreateMatchAsync(match);

            sgfGenerated.MatchId = match.Id; // Associate the SGF record with the newly created match

            await _sgfService.CreateSgfRecord(sgfGenerated);

            await Shell.Current.GoToAsync( $"///{nameof(MatchListPage)}?RoundId={_roundId}&RoundNumber={_roundNumber}");
        }

        public string BuildResult()
{
    if (SelectedWinner == "Draw")
        return "0";

    var color = SelectedWinner == "Black" ? "B" : "W";

    return SelectedResultType switch
    {
        "Resign" => $"{color}+R",
        "Timeout" => $"{color}+T",
        "Other" => $"{color}+O",
        "Points" => $"{color}+{Points}",
        _ => ""
    };
}


        private async Task<SgfRecord> BuildSgfRecord(Match newMatch)
        {
            string autoName = SgfRecord.GenerateAutoName(newMatch.GameDate, _roundNumber, newMatch.BoardNumber, SelectedPlayer1.Name, SelectedPlayer2.Name);

           return new SgfRecord
            {
                Id = Guid.NewGuid(),
                Name = autoName,
                MatchId = newMatch.Id,
                RawSgf = SgfRaw,
                ParsedMovesJson = string.Empty,
                RetrievedAt = DateTime.Now
            };
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
