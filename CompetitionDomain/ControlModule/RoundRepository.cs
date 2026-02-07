using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class RoundRepository : IRoundRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<Round>? roundRepoManager;

        private RoundCollection rounds = new RoundCollection();

        public RoundRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<RoundCollection> LoadCollection(bool reload)
        {
            if (reload || rounds.Count == 0)
            {
                rounds = new RoundCollection();
                roundRepoManager = factory.Get(rounds);
                await roundRepoManager.LoadCollection();
            }

            return rounds;
        }

        public async Task<RoundCollection> GetAllRounds(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<Round?> GetRoundById(Guid id, bool reload = true)
        {
            RoundCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreateRound(Round newRound, bool reload = true)
        {
            newRound.ModifiedDate = DateTime.UtcNow;
            RoundCollection all = await LoadCollection(reload);
            all.Add(newRound);

            await roundRepoManager!.InsertSingleItem(newRound);
            return string.Empty;
        }

        public async Task<string> UpdateRound(Round updatedRound, bool reload = true)
        {
            updatedRound.ModifiedDate = DateTime.UtcNow;
            await LoadCollection(reload);
            await roundRepoManager!.UpdateSingleItem(updatedRound);
            return string.Empty;
        }

        public async Task<string> DeleteRound(Round deleteRound, bool reload = true)
        {
            RoundCollection all = await LoadCollection(reload);
            all.Remove(deleteRound);

            await roundRepoManager!.DeleteSingleItem(deleteRound);
            return string.Empty;
        }
    }
}
