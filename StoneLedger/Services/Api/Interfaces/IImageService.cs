using System;
using System.Collections.Generic;
using System.Text;
using ImageDomain.Model;
using Image = ImageDomain.Model.Image;

namespace StoneLedger.Services.Api.Interfaces
{
    public interface IImageService
    {
        Task<Image?> GetImageByIdAsync(Guid id);
        Task<IEnumerable<Image>> GetImagesForObjectAsync(Guid objectId, int objectType);
        Task<Image?> GetPrimaryImageAsync(Guid objectId, int objectType);

        Task<string> AddImageAsync(Image newImage);
        Task<string> UpdateImageAsync(Image updatedImage);
        Task<string> DeleteImageAsync(Guid id);
    }
}

