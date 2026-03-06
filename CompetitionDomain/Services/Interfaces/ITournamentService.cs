using CompetitionDomain.Model;
using ImageDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<TournamentCollection> GetAllTournamentsAsync();
        Task<Tournament?> GetTournamentByIdAsync(Guid id);

        Task<string> CreateTournamentAsync(Tournament newTournament);
        Task<string> UpdateTournamentAsync(Tournament updatedTournament);
        Task<string> DeleteTournamentAsync(Guid tournamentId);

        // IMAGE METHODS
        Task<ImageCollection> GetImagesForTournamentAsync(Guid tournamentId);
        Task<Image?> GetImageByIdAsync(Guid imageId);
        Task<Image?> GetPrimaryImageAsync(Guid tournamentId);

        Task<string> AddImageAsync(Guid tournamentId, Image newImage);
        Task<string> UpdateImageAsync(Image updatedImage);
        Task<string> DeleteImageAsync(Guid imageId);
    }
}
