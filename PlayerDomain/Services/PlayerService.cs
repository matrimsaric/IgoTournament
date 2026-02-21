using global::PlayerDomain.ControlModule.Interfaces;
using global::PlayerDomain.Model;
using ImageDomain.Model;
using PlayerDomain.Services.Interfaces;

namespace PlayerDomain.Services
{
       public class PlayerService : IPlayerService
        {
            private readonly IPlayerRepository _repo;

            public PlayerService(IPlayerRepository repo)
            {
                _repo = repo;
            }

            public Task<PlayerCollection> GetAllPlayersAsync()
                => _repo.GetAllPlayers();

            public Task<Player?> GetPlayerByIdAsync(Guid id)
                => _repo.GetPlayerById(id);

            public Task<string> CreatePlayerAsync(Player newPlayer)
                => _repo.CreatePlayer(newPlayer);

            public Task<string> UpdatePlayerAsync(Player updatedPlayer)
                => _repo.UpdatePlayer(updatedPlayer);

            public Task<string> DeletePlayerAsync(Guid playerId)
                => _repo.DeletePlayer(new Player { Id = playerId });

            // IMAGE METHODS
            public Task<ImageCollection> GetImagesForPlayerAsync(Guid playerId)
                => _repo.GetImages(playerId);

            public Task<Image?> GetImageByIdAsync(Guid imageId)
                => _repo.GetImage(imageId);

            public Task<Image?> GetPrimaryImageAsync(Guid playerId)
                => _repo.GetPrimaryImageForPlayer(playerId);

            public Task<string> AddImageAsync(Guid playerId, Image newImage)
                => _repo.AddImage(playerId, newImage);

            public Task<string> UpdateImageAsync(Image updatedImage)
                => _repo.UpdateImage(updatedImage);

            public Task<string> DeleteImageAsync(Guid imageId)
                => _repo.DeleteImage(new Image { Id = imageId });
        }
    }
