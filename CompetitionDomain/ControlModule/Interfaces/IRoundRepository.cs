using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface IRoundRepository
    {
        Task<RoundCollection> GetAllRounds(bool reload = true);
        Task<Round?> GetRoundById(Guid id, bool reload = true);

        Task<string> CreateRound(Round newRound, bool reload = true);
        Task<string> UpdateRound(Round updatedRound, bool reload = true);
        Task<string> DeleteRound(Round deleteRound, bool reload = true);
    }
}
