using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;
using ImageDomain.Model;

namespace CompetitionDomain.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _repo;

        public TournamentService(ITournamentRepository repo)
        {
            _repo = repo;
        }

        public Task<TournamentCollection> GetAllTournamentsAsync()
            => _repo.GetAllTournaments();

        public Task<Tournament?> GetTournamentByIdAsync(Guid id)
            => _repo.GetTournamentById(id);

        public Task<string> CreateTournamentAsync(Tournament newTournament)
            => _repo.CreateTournament(newTournament);

        public Task<string> UpdateTournamentAsync(Tournament updatedTournament)
            => _repo.UpdateTournament(updatedTournament);

        public Task<string> DeleteTournamentAsync(Guid tournamentId)
            => _repo.DeleteTournament(new Tournament { Id = tournamentId });

        // IMAGE METHODS
        public Task<ImageCollection> GetImagesForTournamentAsync(Guid tournamentId)
            => _repo.GetImages(tournamentId);

        public Task<Image?> GetImageByIdAsync(Guid imageId)
            => _repo.GetImage(imageId);

        public Task<Image?> GetPrimaryImageAsync(Guid tournamentId)
            => _repo.GetPrimaryImageForTournament(tournamentId);

        public Task<string> AddImageAsync(Guid tournamentId, Image newImage)
            => _repo.AddImage(tournamentId, newImage);

        public Task<string> UpdateImageAsync(Image updatedImage)
            => _repo.UpdateImage(updatedImage);

        public Task<string> DeleteImageAsync(Guid imageId)
            => _repo.DeleteImage(new Image { Id = imageId });
    }
}
