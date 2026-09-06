
using System.Diagnostics;
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
            var response = await _http.GetAsync($"api/content/images/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[ImageService] GetImageByIdAsync({id}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<Image>();
        }

        public async Task<IEnumerable<Image>> GetImagesForObjectAsync(Guid objectId, int objectType)
        {
            var response = await _http.GetAsync($"api/content/images/object/{objectId}/{objectType}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[ImageService] GetImagesForObjectAsync({objectId}, {objectType}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return Enumerable.Empty<Image>();
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<Image>>() ?? Enumerable.Empty<Image>();
        }

        public async Task<Image> GetTeamImagesForObjectAsync(Guid id)
        {
            var response = await _http.GetAsync($"api/content/players/{id}/team-image");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[ImageService] GetTeamImagesForObjectAsync({id}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<Image>() ?? default;
        }

        public async Task<Image?> GetPrimaryImageAsync(Guid objectId, int objectType)
        {
            var response = await _http.GetAsync($"api/content/images/object/{objectId}/{objectType}/primary");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[ImageService] GetPrimaryImageAsync({objectId}, {objectType}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<Image>();
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

