using CompetitionDomain.Model;
using ImageDomain.Model;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = ImageDomain.Model.Image;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamCollection> GetAllTeams(bool reload = true);
        Task<Team?> GetTeamById(Guid id, bool reload = true);

        Task<string> CreateTeam(Team newTeam, bool reload = true);
        Task<string> UpdateTeam(Team updatedTeam, bool reload = true);
        Task<string> DeleteTeam(Team deleteTeam, bool reload = true);

        // ------------------------------------------------------------
        // IMAGE ACCESS METHODS
        // ------------------------------------------------------------
        Task<ImageCollection> GetImages(Guid teamId, bool reload = true);
        Task<Image?> GetImage(Guid imageId, bool reload = true);
        Task<Image?> GetPrimaryImageForTeam(Guid teamId, bool reload = true);

        Task<string> AddImage(Guid teamId, Image newImage, bool reload = true);
        Task<string> UpdateImage(Image updatedImage, bool reload = true);
        Task<string> DeleteImage(Image deleteImage, bool reload = true);
    }
}
