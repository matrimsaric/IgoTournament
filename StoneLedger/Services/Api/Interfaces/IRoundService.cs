using CompetitionDomain.Model;
using StoneLedger.Models;

namespace StoneLedger.Services.Api
{
    public interface IRoundService
    {
        Task<List<Round>> GetRoundsForTournamentAsync(Guid tournamentId);

        Task<Round> CreateRoundAsync(RoundDto newRound);
    }
}
