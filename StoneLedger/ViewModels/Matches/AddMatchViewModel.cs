using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Matches
{
    public class AddMatchViewModel : BaseViewModel
    {
        private readonly MatchService _matchService;

        public int BoardNumber { get; set; }
        public string BlackPlayerId { get; set; }
        public string WhitePlayerId { get; set; }
        public string Result { get; set; }
        public string SgfPath { get; set; }

        public Guid RoundId { get; set; } // set via navigation later

        public ICommand SaveCommand { get; }

        public AddMatchViewModel(MatchService matchService)
        {
            _matchService = matchService;
            SaveCommand = new Command(async () => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            var match = new Match
            {
                Id = Guid.NewGuid(),
                RoundId = RoundId,
                BoardNumber = BoardNumber,
                BlackPlayerId = Guid.Parse(BlackPlayerId),
                WhitePlayerId = Guid.Parse(WhitePlayerId),
                Result = Result,
                GameDate = DateTime.Today,
                Name = "TEMP NAME" // will be auto‑generated later
            };

            await _matchService.CreateMatchAsync(match);

            await Shell.Current.GoToAsync("..");
        }
    }
}
