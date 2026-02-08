using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface ITeamMembershipRepository
    {
        Task<TeamMembershipCollection> GetAllMemberships(bool reload = true);
        Task<TeamMembership?> GetMembershipById(Guid id, bool reload = true);

        Task<string> AddMembership(TeamMembership membership, bool reload = true);
        Task<string> UpdateMembership(TeamMembership membership, bool reload = true);
        Task<string> RemoveMembership(TeamMembership membership, bool reload = true);

        Task<TeamMembershipCollection> GetMembershipsForTeam(Guid teamId, bool reload = true);
        Task<TeamMembershipCollection> GetMembershipsForPlayer(Guid playerId, bool reload = true);
        Task<TeamMembershipCollection> GetMembershipsForSeason(string season, bool reload = true);
    }
}
