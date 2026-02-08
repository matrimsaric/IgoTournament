using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ImageDomain.ControlModule
{
    public class ImageRepository : IImageRepository
    {
        private readonly IRepositoryFactory factory;
        private IRepositoryManager<Image>? imageRepoManager;
        private readonly IDbUtilityFactory dbUtilityFactory;

        private ImageCollection images = new ImageCollection();

        public ImageRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<ImageCollection> LoadCollection(bool reload)
        {
            if (reload || images.Count == 0)
            {
                images = new ImageCollection();
                imageRepoManager = factory.Get(images);
                await imageRepoManager.LoadCollection();
            }

            return images;
        }

        public async Task<ImageCollection> GetImagesForObject(Guid objectId, int objectType, bool reload = true)
        {
            ImageCollection all = await LoadCollection(reload);
            var filtered = new ImageCollection();

            foreach (var img in all.Where(x => x.ObjectId == objectId && x.ObjectType == objectType))
                filtered.Add(img);

            return filtered;
        }

        public async Task<Image?> GetImageById(Guid id, bool reload = true)
        {
            ImageCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<string> CreateImage(Image newImage, bool reload = true)
        {
            ImageCollection all = await LoadCollection(reload);
            all.Add(newImage);

            await imageRepoManager!.InsertSingleItem(newImage);
            return string.Empty;
        }

        public async Task<string> UpdateImage(Image updatedImage, bool reload = true)
        {
            await imageRepoManager!.UpdateSingleItem(updatedImage);
            return string.Empty;
        }

        public async Task<string> DeleteImage(Image deleteImage, bool reload = true)
        {
            ImageCollection all = await LoadCollection(reload);
            all.Remove(deleteImage);

            await imageRepoManager!.DeleteSingleItem(deleteImage);
            return string.Empty;
        }
    }
}
