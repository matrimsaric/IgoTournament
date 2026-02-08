using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ImageDomain.ControlModule
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository repository;

        public ImageService(IImageRepository repo)
        {
            repository = repo;
        }

        public Task<ImageCollection> GetImagesForObject(Guid objectId, int objectType, bool reload = true)
            => repository.GetImagesForObject(objectId, objectType, reload);

        public Task<Image?> GetImageById(Guid id, bool reload = true)
            => repository.GetImageById(id, reload);

        public Task<string> AddImage(Image newImage, bool reload = true)
            => repository.CreateImage(newImage, reload);

        public Task<string> UpdateImage(Image updatedImage, bool reload = true)
            => repository.UpdateImage(updatedImage, reload);

        public Task<string> DeleteImage(Image deleteImage, bool reload = true)
            => repository.DeleteImage(deleteImage, reload);

        // ------------------------------------------------------------
        // PRIMARY IMAGE LOGIC
        // ------------------------------------------------------------
        public async Task<Image?> GetPrimaryImageForObject(Guid objectId, int objectType, bool reload = true)
        {
            var images = await repository.GetImagesForObject(objectId, objectType, reload);

            if (images.Count == 0)
                return null;

            // Primary = lowest sort_order
            return images.OrderBy(x => x.SortOrder).FirstOrDefault();
        }
    }
}
