using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Services
{
    public class TeamMembershipService : ITeamMembershipService
    {
        private readonly ITeamMembershipRepository _repo;

        public TeamMembershipService(ITeamMembershipRepository repo)
        {
            _repo = repo;
        }
        public async Task<TeamMembership?> GetCurrentMembershipForPlayerAsync(Guid playerId)
        {
            // You may want to filter by season later
            var memberships = await _repo.GetMembershipsForPlayer(playerId);
            return memberships
                .OrderByDescending(m => m.Season) // or whatever logic defines "current"
                .FirstOrDefault();
        }

    }
}
