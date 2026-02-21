using ImageDomain.Model;
using PlayerDomain.Model;
using System;
using System.Threading.Tasks;

namespace PlayerDomain.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerCollection> GetAllPlayersAsync();
        Task<Player?> GetPlayerByIdAsync(Guid id);

        Task<string> CreatePlayerAsync(Player newPlayer);
        Task<string> UpdatePlayerAsync(Player updatedPlayer);
        Task<string> DeletePlayerAsync(Guid playerId);

        // IMAGE METHODS
        Task<ImageCollection> GetImagesForPlayerAsync(Guid playerId);
        Task<Image?> GetImageByIdAsync(Guid imageId);
        Task<Image?> GetPrimaryImageAsync(Guid playerId);
        Task<string> AddImageAsync(Guid playerId, Image newImage);
        Task<string> UpdateImageAsync(Image updatedImage);
        Task<string> DeleteImageAsync(Guid imageId);
    }
}
