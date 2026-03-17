using CompetitionDomain.Model;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class MatchService : IMatchService
    {
        private readonly HttpClient _httpClient;

        public MatchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Match>> GetMatchesForRoundAsync(Guid roundId)
        {
            var url = $"api/content/rounds/{roundId}/matches";
            var result = await _httpClient.GetFromJsonAsync<List<Match>>(url);

            return result ?? new List<Match>();
        }

        public async Task CreateMatchAsync(Match newMatch)
        {
            var response = await _httpClient.PostAsJsonAsync("api/content/matches", newMatch);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Match?> GetMatchByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/content/matches/{id}");

            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadFromJsonAsync<Match>();


        }
    }
}
