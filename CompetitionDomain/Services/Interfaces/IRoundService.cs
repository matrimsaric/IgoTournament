using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.Services.Interfaces
{
    public interface IRoundService
    {
        Task<RoundCollection> GetAllRoundsAsync();
        Task<Round?> GetRoundByIdAsync(Guid id);

        Task<string> CreateRoundAsync(Round newRound);
        Task<string> UpdateRoundAsync(Round updatedRound);
        Task<string> DeleteRoundAsync(Guid roundId);
    }
}
