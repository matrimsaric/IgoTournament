using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;

public class MatchContentViewModel : BaseViewModel
{
    private readonly MatchService _matchService;

    private Match _match;
    public Match Match
    {
        get => _match;
        set => SetProperty(ref _match, value);
    }

    public MatchContentViewModel(MatchService matchService)
    {
        _matchService = matchService;
    }

    public async Task LoadMatchAsync(Guid matchId)
    {
        if (matchId == Guid.Empty)
            return;

        var match = await _matchService.GetMatchByIdAsync(matchId);

        if (match == null)
            return;

        Match = match;
    }
}
