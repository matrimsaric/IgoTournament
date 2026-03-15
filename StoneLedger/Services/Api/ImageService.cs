
using System.Net.Http.Json;
using ImageDomain.Model;
using StoneLedger.Services.Api.Interfaces;
using Image = ImageDomain.Model.Image;

namespace StoneLedger.Services.Api
{
    public class ImageService
    {
        private readonly HttpClient _http;

        public ImageService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Image?> GetImageByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<Image>($"api/content/images/{id}");
        }

        public async Task<IEnumerable<Image>> GetImagesForObjectAsync(Guid objectId, int objectType)
        {
            return await _http.GetFromJsonAsync<IEnumerable<Image>>(
                $"api/content/images/object/{objectId}/{objectType}"
            ) ?? Enumerable.Empty<Image>();
        }

        public async Task<Image?> GetPrimaryImageAsync(Guid objectId, int objectType)
        {
            return await _http.GetFromJsonAsync<Image>(
                $"api/content/images/object/{objectId}/{objectType}/primary"
            );
        }

        public async Task<string> AddImageAsync(Image newImage)
        {
            var response = await _http.PostAsJsonAsync("api/content/images", newImage);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateImageAsync(Image updatedImage)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/content/images/{updatedImage.Id}",
                updatedImage
            );
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteImageAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/content/images/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}

