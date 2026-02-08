using CommonModule.Enums;
using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ImageDomain.ControlModule;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using ServerCommonModule.Configuration;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IRepositoryFactory factory;
        private IRepositoryManager<Team>? teamRepoManager;
        private readonly IDbUtilityFactory dbUtilityFactory;

        private readonly IImageService imageService;

        private TeamCollection teams = new TeamCollection();

        public TeamRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);

            imageService = new ImageService(new ImageRepository(env, dbFactory));
        }

        private async Task<TeamCollection> LoadCollection(bool reload)
        {
            if (reload || teams.Count == 0)
            {
                teams = new TeamCollection();
                teamRepoManager = factory.Get(teams);
                await teamRepoManager.LoadCollection();
            }

            return teams;
        }

        public async Task<TeamCollection> GetAllTeams(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<Team?> GetTeamById(Guid id, bool reload = true)
        {
            TeamCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreateTeam(Team newTeam, bool reload = true)
        {
            string status = await CheckForDuplicates(newTeam, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            TeamCollection all = await LoadCollection(reload);
            all.Add(newTeam);

            await teamRepoManager!.InsertSingleItem(newTeam);
            return string.Empty;
        }

        public async Task<string> UpdateTeam(Team updatedTeam, bool reload = true)
        {
            string status = await CheckForDuplicates(updatedTeam, reload);
            if (!string.IsNullOrEmpty(status))
                return status;

            await teamRepoManager!.UpdateSingleItem(updatedTeam);
            return string.Empty;
        }

        public async Task<string> DeleteTeam(Team deleteTeam, bool reload = true)
        {
            TeamCollection all = await LoadCollection(reload);
            all.Remove(deleteTeam);

            await teamRepoManager!.DeleteSingleItem(deleteTeam);
            return string.Empty;
        }

        private async Task<string> CheckForDuplicates(Team team, bool reload)
        {
            TeamCollection all = await LoadCollection(reload);

            bool duplicate =
                all.Any(x =>
                    x.Name == team.Name &&
                    x.Id != team.Id);

            return duplicate ? "Duplicate team detected." : string.Empty;
        }

        // ------------------------------------------------------------
        // IMAGE ACCESS METHODS
        // ------------------------------------------------------------

        public Task<ImageCollection> GetImages(Guid teamId, bool reload = true)
        {
            return imageService.GetImagesForObject(
                teamId,
                (int)ImageObjectType.Team,
                reload
            );
        }

        public Task<Image?> GetImage(Guid imageId, bool reload = true)
        {
            return imageService.GetImageById(imageId, reload);
        }

        public Task<Image?> GetPrimaryImageForTeam(Guid teamId, bool reload = true)
        {
            return imageService.GetPrimaryImageForObject(
                teamId,
                (int)ImageObjectType.Team,
                reload
            );
        }

        public Task<string> AddImage(Guid teamId, Image newImage, bool reload = true)
        {
            newImage.ObjectId = teamId;
            newImage.ObjectType = (int)ImageObjectType.Team;

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
    }
}
