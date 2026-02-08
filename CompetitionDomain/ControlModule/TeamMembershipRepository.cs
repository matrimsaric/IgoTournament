using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class TeamMembershipRepository : ITeamMembershipRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<TeamMembership>? membershipRepoManager;

        private TeamMembershipCollection memberships = new TeamMembershipCollection();

        public TeamMembershipRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<TeamMembershipCollection> LoadCollection(bool reload)
        {
            if (reload || memberships.Count == 0)
            {
                memberships = new TeamMembershipCollection();
                membershipRepoManager = factory.Get(memberships);
                await membershipRepoManager.LoadCollection();
            }

            return memberships;
        }

        public async Task<TeamMembershipCollection> GetAllMemberships(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<TeamMembership?> GetMembershipById(Guid id, bool reload = true)
        {
            TeamMembershipCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> AddMembership(TeamMembership membership, bool reload = true)
        {
            string status = await CheckForDuplicates(membership, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            TeamMembershipCollection all = await LoadCollection(reload);
            all.Add(membership);

            await membershipRepoManager!.InsertSingleItem(membership);
            return string.Empty;
        }

        public async Task<string> UpdateMembership(TeamMembership membership, bool reload = true)
        {
            string status = await CheckForDuplicates(membership, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            await membershipRepoManager!.UpdateSingleItem(membership);
            return string.Empty;
        }

        public async Task<string> RemoveMembership(TeamMembership membership, bool reload = true)
        {
            TeamMembershipCollection all = await LoadCollection(reload);
            all.Remove(membership);

            await membershipRepoManager!.DeleteSingleItem(membership);
            return string.Empty;
        }

        public async Task<TeamMembershipCollection> GetMembershipsForTeam(Guid teamId, bool reload = true)
        {
            TeamMembershipCollection all = await LoadCollection(reload);
            var filtered = new TeamMembershipCollection();

            foreach (var m in all.Where(x => x.TeamId == teamId))
                filtered.Add(m);

            return filtered;
        }

        public async Task<TeamMembershipCollection> GetMembershipsForPlayer(Guid playerId, bool reload = true)
        {
            TeamMembershipCollection all = await LoadCollection(reload);
            var filtered = new TeamMembershipCollection();

            foreach (var m in all.Where(x => x.PlayerId == playerId))
                filtered.Add(m);

            return filtered;
        }

        public async Task<TeamMembershipCollection> GetMembershipsForSeason(string season, bool reload = true)
        {
            TeamMembershipCollection all = await LoadCollection(reload);
            var filtered = new TeamMembershipCollection();

            foreach (var m in all.Where(x => x.Season == season))
                filtered.Add(m);

            return filtered;
        }

        private async Task<string> CheckForDuplicates(TeamMembership membership, bool reload)
        {
            TeamMembershipCollection all = await LoadCollection(reload);

            bool duplicate =
                all.Any(x =>
                    x.PlayerId == membership.PlayerId &&
                    x.TeamId == membership.TeamId &&
                    x.Season == membership.Season &&
                    x.Id != membership.Id);

            return duplicate ? "Duplicate membership detected." : string.Empty;
        }
    }
}
