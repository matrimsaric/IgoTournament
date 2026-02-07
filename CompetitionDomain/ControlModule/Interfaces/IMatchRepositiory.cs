using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface IMatchRepository
    {
        Task<MatchCollection> GetAllMatches(bool reload = true);
        Task<Match?> GetMatchById(Guid id, bool reload = true);

        Task<string> CreateMatch(Match newMatch, bool reload = true);
        Task<string> UpdateMatch(Match updatedMatch, bool reload = true);
        Task<string> DeleteMatch(Match deleteMatch, bool reload = true);
    }
}
