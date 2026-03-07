using CompetitionDomain.Model;

namespace StoneLedger.Services.Api
{
    public interface IRoundService
    {
        Task<List<Round>> GetRoundsForTournamentAsync(Guid tournamentId);

        Task<Round> CreateRoundAsync(Round newRound);
    }
}
