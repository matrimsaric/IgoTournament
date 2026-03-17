using System.Net.Http.Json;
using CommonModule.Enums;
using PlayerDomain.Model;
using PlayerDomain.Services.Interfaces;
using StoneLedger.Models;

namespace StoneLedger.Services.Api
{
    public class PlayerService
    {
        private readonly HttpClient _http;

        public PlayerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            var result = await _http.GetFromJsonAsync<List<Player>>("/api/content/players");
            return result ?? new List<Player>();
        }

        public async Task<Player?> GetPlayerByIdAsync(Guid id)
        {
            var response = await _http.GetAsync($"api/content/players/{id}");

            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadFromJsonAsync<Player>();
        }

        public async Task<Image?> GetPlayerImageByIdAsync(Guid id, ImageSizeType imgSize)
        {
            var response = await _http.GetAsync($"api/content/images/object/{id}/{imgSize}");

            if (!response.IsSuccessStatusCode)
                return new Image();

            return await response.Content.ReadFromJsonAsync<Image>()
                   ?? new Image();
        }
    }
}
