using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.Services.Interfaces
{
    public interface IMatchService
    {
        Task<MatchCollection> GetAllMatchesAsync();
        Task<Match?> GetMatchByIdAsync(Guid id);

        Task<string> CreateMatchAsync(Match newMatch);
        Task<string> UpdateMatchAsync(Match updatedMatch);
        Task<string> DeleteMatchAsync(Guid matchId);
    }
}
