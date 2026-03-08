using System.Net.Http.Json;
using PlayerDomain.Model;
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
    }
}
