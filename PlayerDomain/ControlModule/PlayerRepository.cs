using CommonModule.Enums;
using ImageDomain.ControlModule;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using PlayerDomain.ControlModule.Interfaces;
using PlayerDomain.Model;
using ServerCommonModule.Configuration;
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
        private readonly IImageService imageService;

        private PlayerCollection players = new PlayerCollection();

        public PlayerRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
            imageService = new ImageService(new ImageRepository(env, dbUtilityFactory));
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

        public Task<ImageCollection> GetImages(Guid playerId, bool reload = true)
    => imageService.GetImagesForObject(playerId, (int)ImageObjectType.Player, reload);

        public Task<Image?> GetImage(Guid imageId, bool reload = true)
            => imageService.GetImageById(imageId, reload);

        public Task<string> AddImage(Guid playerId, Image newImage, bool reload = true)
        {
            newImage.ObjectId = playerId;
            newImage.ObjectType = (int)ImageObjectType.Player;
            return imageService.AddImage(newImage, reload);
        }

        public Task<string> UpdateImage(Image updatedImage, bool reload = true)
            => imageService.UpdateImage(updatedImage, reload);

        public Task<string> DeleteImage(Image deleteImage, bool reload = true)
            => imageService.DeleteImage(deleteImage, reload);

        public Task<Image?> GetPrimaryImageForPlayer(Guid playerId, bool reload = true)
        {
            return imageService.GetPrimaryImageForObject(
                playerId,
                (int)ImageObjectType.Player,
                reload
            );
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
