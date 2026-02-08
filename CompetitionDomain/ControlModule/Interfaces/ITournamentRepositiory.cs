using CompetitionDomain.Model;
using ImageDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface ITournamentRepository
    {
        Task<TournamentCollection> GetAllTournaments(bool reload = true);
        Task<Tournament?> GetTournamentById(Guid id, bool reload = true);

        Task<string> CreateTournament(Tournament newTournament, bool reload = true);
        Task<string> UpdateTournament(Tournament updatedTournament, bool reload = true);
        Task<string> DeleteTournament(Tournament deleteTournament, bool reload = true);

        Task<ImageCollection> GetImages(Guid tournamentId, bool reload = true);
        Task<Image?> GetImage(Guid imageId, bool reload = true);
        Task<Image?> GetPrimaryImageForTournament(Guid tournamentId, bool reload = true);

        Task<string> AddImage(Guid tournamentId, Image newImage, bool reload = true);
        Task<string> UpdateImage(Image updatedImage, bool reload = true);
        Task<string> DeleteImage(Image deleteImage, bool reload = true);
    }
}
