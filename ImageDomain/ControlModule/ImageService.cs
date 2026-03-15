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

        public async Task<string> AddImage(Image newImage, bool reload = true)
        {
            // Load only images for this object
            ImageCollection all = await repository.GetImagesForObject(
                newImage.ObjectId,
                newImage.ObjectType,
                reload
            );

            // Look for an existing image with the same SizeType
            var existing = all.FirstOrDefault(img =>
                img.SizeType == newImage.SizeType
            );

            if (existing != null)
            {
                // Overwrite existing image
                newImage.Id = existing.Id;

                // Update in-memory collection
                //all.Replace(existing, newImage);

                // Persist update
                await repository!.UpdateImage(newImage);
                return string.Empty;
            }

            // Otherwise insert new
            all.Add(newImage);
            await repository!.CreateImage(newImage);
            return string.Empty;
        }

        //    => repository.CreateImage(newImage, reload);

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
