using PlayerDomain.ControlModule.Interfaces;
using PlayerDomain.Model;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using TournamentDomain.ControlModule.Interfaces;
//using TournamentDomain.Model;

namespace PlayerDomain.ControlModule
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IRepositoryFactory factory;
        private IRepositoryManager<Player>? playerRepoManager;
        private readonly IDbUtilityFactory dbUtilityFactory;

        private PlayerCollection players = new PlayerCollection();

        public PlayerRepository()
        {
            IEnvironmentalParameters env = new EnvironmentalParameters();
            env.ConnectionString = "Host=localhost;Username=postgres;Password=modena;Database=IgoTournament";
            env.DatabaseType = "PostgreSQL";

            dbUtilityFactory = new PgUtilityFactory(env, null);
            factory = new RepositoryFactory(dbUtilityFactory, env);
        }

        public PlayerRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<PlayerCollection> LoadCollection(bool reload)
        {
            if (reload || players.Count == 0)
            {
                players = new PlayerCollection();
                playerRepoManager = factory.Get(players);
                await playerRepoManager.LoadCollection();
            }

            return players;
        }

        public async Task<PlayerCollection> GetAllPlayers(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<Player?> GetPlayerById(Guid id, bool reload = true)
        {
            PlayerCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreatePlayer(Player newPlayer, bool reload = true)
        {
            string status = await CheckForDuplicates(newPlayer, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            PlayerCollection all = await LoadCollection(reload);
            all.Add(newPlayer);

            await playerRepoManager!.InsertSingleItem(newPlayer);
            return string.Empty;
        }

        public async Task<string> UpdatePlayer(Player updated, bool reload = true)
        {
            string status = await CheckForDuplicates(updated, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            await playerRepoManager!.UpdateSingleItem(updated);
            return string.Empty;
        }

        public async Task<string> DeletePlayer(Player deleteMe, bool reload = true)
        {
            PlayerCollection all = await LoadCollection(reload);
            all.Remove(deleteMe);

            await playerRepoManager!.DeleteSingleItem(deleteMe);
            return string.Empty;
        }

        private async Task<string> CheckForDuplicates(Player player, bool reload)
        {
            PlayerCollection all = await LoadCollection(reload);

            bool duplicate =
                all.Any(x =>
                    (x.Name == player.Name || x.Rank == player.Rank) &&
                    x.Id != player.Id);

            return duplicate ? "Duplicate player detected." : string.Empty;
        }
    }
}
