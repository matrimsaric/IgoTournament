using ImageDomain.Model;
using System;
using System.Threading.Tasks;

namespace ImageDomain.ControlModule.Interfaces
{
    public interface IImageRepository
    {
        Task<ImageCollection> GetImagesForObject(Guid objectId, int objectType, bool reload = true);
        Task<Image?> GetImageById(Guid id, bool reload = true);

        Task<string> CreateImage(Image newImage, bool reload = true);
        Task<string> UpdateImage(Image updatedImage, bool reload = true);
        Task<string> DeleteImage(Image deleteImage, bool reload = true);
    }
}
