using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<Match>? matchRepoManager;

        private MatchCollection matches = new MatchCollection();

        public MatchRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<MatchCollection> LoadCollection(bool reload)
        {
            if (reload || matches.Count == 0)
            {
                matches = new MatchCollection();
                matchRepoManager = factory.Get(matches);
                await matchRepoManager.LoadCollection();
            }

            return matches;
        }

        public async Task<MatchCollection> GetAllMatches(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<Match?> GetMatchById(Guid id, bool reload = true)
        {
            MatchCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreateMatch(Match newMatch, bool reload = true)
        {
            MatchCollection all = await LoadCollection(reload);
            all.Add(newMatch);

            newMatch.ModifiedDate = DateTime.UtcNow;
            string test = newMatch.GameDate.ToString();
            await matchRepoManager!.InsertSingleItem(newMatch);
            return string.Empty;
        }

        public async Task<string> UpdateMatch(Match updated, bool reload = true)
        {
            await LoadCollection(reload);
            updated.ModifiedDate = DateTime.UtcNow;
            await matchRepoManager!.UpdateSingleItem(updated);
            return string.Empty;
        }

        public async Task<string> DeleteMatch(Match deleteMe, bool reload = true)
        {
            MatchCollection all = await LoadCollection(reload);
            all.Remove(deleteMe);

            await matchRepoManager!.DeleteSingleItem(deleteMe);
            return string.Empty;
        }
    }
}
