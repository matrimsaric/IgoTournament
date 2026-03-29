using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Services.Interfaces
{
    public interface ITeamMembershipService
    {
        Task<TeamMembership?> GetCurrentMembershipForPlayerAsync(Guid playerId);
    }
}
