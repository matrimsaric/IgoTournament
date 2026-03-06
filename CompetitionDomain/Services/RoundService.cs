using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;

namespace CompetitionDomain.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _repo;

        public RoundService(IRoundRepository repo)
        {
            _repo = repo;
        }

        public Task<RoundCollection> GetAllRoundsAsync()
            => _repo.GetAllRounds();

        public Task<Round?> GetRoundByIdAsync(Guid id)
            => _repo.GetRoundById(id);

        public Task<string> CreateRoundAsync(Round newRound)
            => _repo.CreateRound(newRound);

        public Task<string> UpdateRoundAsync(Round updatedRound)
            => _repo.UpdateRound(updatedRound);

        public Task<string> DeleteRoundAsync(Guid roundId)
            => _repo.DeleteRound(new Round { Id = roundId });
    }
}
