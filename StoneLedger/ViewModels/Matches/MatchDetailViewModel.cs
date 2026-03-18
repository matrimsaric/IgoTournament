using CompetitionDomain.Services.Interfaces;
using PlayerDomain.Services.Interfaces;
using StoneLedger.Services.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.ViewModels.Matches
{
    public class MatchDetailViewModel : BaseViewModel
    {
        private readonly MatchService _matchService;
        public Guid PlayerAId { get; private set; }
        public Guid PlayerBId { get; private set; }

        private Guid _matchId;
        public Guid MatchId
        {
            get => _matchId;
            set => SetProperty(ref _matchId, value);
        }


        public MatchDetailViewModel(MatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task LoadMatchAsync(Guid matchId)
        {
            var match = await _matchService.GetMatchByIdAsync(matchId);

            PlayerAId = match.BlackPlayerId;
            PlayerBId = match.WhitePlayerId;

            MatchId = match.Id;
            OnPropertyChanged(nameof(MatchId));

            OnPropertyChanged(nameof(PlayerAId));
            OnPropertyChanged(nameof(PlayerBId));
        }

    }
}
