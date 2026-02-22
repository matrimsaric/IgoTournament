using System.Net.Http.Json;
using StoneLedger.Models;

namespace StoneLedger.Services.Api
{
    public class PlayerService
    {
        private readonly HttpClient _http;

        public PlayerService()
        {
            // You can move this base address to config later
            _http = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            Console.WriteLine("Using API base: " + _http.BaseAddress);
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            var result = await _http.GetFromJsonAsync<List<Player>>("/api/content/players");

            return result ?? new List<Player>();
        }
    }
}
