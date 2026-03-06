using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;

namespace CompetitionDomain.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _repo;

        public MatchService(IMatchRepository repo)
        {
            _repo = repo;
        }

        public Task<MatchCollection> GetAllMatchesAsync()
            => _repo.GetAllMatches();

        public Task<Match?> GetMatchByIdAsync(Guid id)
            => _repo.GetMatchById(id);

        public Task<string> CreateMatchAsync(Match newMatch)
            => _repo.CreateMatch(newMatch);

        public Task<string> UpdateMatchAsync(Match updatedMatch)
            => _repo.UpdateMatch(updatedMatch);

        public Task<string> DeleteMatchAsync(Guid matchId)
            => _repo.DeleteMatch(new Match { Id = matchId });
    }
}
