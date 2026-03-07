using CompetitionDomain.Model;

namespace StoneLedger.Services.Api
{
    public interface IMatchService
    {
        Task<List<Match>> GetMatchesForRoundAsync(Guid roundId);

        Task<Match> CreateMatchAsync(Match newMatch);
    }
}
