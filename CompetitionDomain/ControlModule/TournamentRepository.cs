using CommonModule.Enums;
using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ImageDomain.ControlModule;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<Tournament>? tournamentRepoManager;
        private readonly IImageService imageService;

        private TournamentCollection tournaments = new TournamentCollection();

        public TournamentRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
            imageService = new ImageService(new ImageRepository(env, dbFactory));
        }

        private async Task<TournamentCollection> LoadCollection(bool reload)
        {
            if (reload || tournaments.Count == 0)
            {
                tournaments = new TournamentCollection();
                tournamentRepoManager = factory.Get(tournaments);
                await tournamentRepoManager.LoadCollection();
            }

            return tournaments;
        }

        public async Task<TournamentCollection> GetAllTournaments(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<Tournament?> GetTournamentById(Guid id, bool reload = true)
        {
            TournamentCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreateTournament(Tournament newTournament, bool reload = true)
        {
            string status = await CheckForDuplicates(newTournament, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            TournamentCollection all = await LoadCollection(reload);
            all.Add(newTournament);

            await tournamentRepoManager!.InsertSingleItem(newTournament);
            return string.Empty;
        }

        public async Task<string> UpdateTournament(Tournament updatedTournament, bool reload = true)
        {
            string status = await CheckForDuplicates(updatedTournament, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            await tournamentRepoManager!.UpdateSingleItem(updatedTournament);
            return string.Empty;
        }

        public async Task<string> DeleteTournament(Tournament deleteTournament, bool reload = true)
        {
            TournamentCollection all = await LoadCollection(reload);
            all.Remove(deleteTournament);

            await tournamentRepoManager!.DeleteSingleItem(deleteTournament);
            return string.Empty;
        }

        // ------------------------------------------------------------
        // IMAGE SUPPORT
        // ------------------------------------------------------------

        public Task<ImageCollection> GetImages(Guid tournamentId, bool reload = true)
        {
            return imageService.GetImagesForObject(
                tournamentId,
                (int)ImageObjectType.Tournament,
                reload
            );
        }

        public Task<Image?> GetImage(Guid imageId, bool reload = true)
        {
            return imageService.GetImageById(imageId, reload);
        }

        public Task<Image?> GetPrimaryImageForTournament(Guid tournamentId, bool reload = true)
        {
            return imageService.GetPrimaryImageForObject(
                tournamentId,
                (int)ImageObjectType.Tournament,
                reload
            );
        }

        public Task<string> AddImage(Guid tournamentId, Image newImage, bool reload = true)
        {
            newImage.ObjectId = tournamentId;
            newImage.ObjectType = (int)ImageObjectType.Tournament;

            return imageService.AddImage(newImage, reload);
        }

        public Task<string> UpdateImage(Image updatedImage, bool reload = true)
        {
            return imageService.UpdateImage(updatedImage, reload);
        }

        public Task<string> DeleteImage(Image deleteImage, bool reload = true)
        {
            return imageService.DeleteImage(deleteImage, reload);
        }

        private async Task<string> CheckForDuplicates(Tournament tournament, bool reload)
        {
            TournamentCollection all = await LoadCollection(reload);

            bool duplicate =
                all.Any(x =>
                    x.Name == tournament.Name &&
                    x.Season == tournament.Season &&
                    x.Id != tournament.Id);

            return duplicate ? "Duplicate tournament detected." : string.Empty;
        }
    }
}
