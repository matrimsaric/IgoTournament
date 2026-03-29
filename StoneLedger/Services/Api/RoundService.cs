using CompetitionDomain.Model;
using StoneLedger.Models;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class RoundService : IRoundService
    {
        private readonly HttpClient _http;

        public RoundService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Round>> GetRoundsForTournamentAsync(Guid tournamentId)
        {
            var result = await _http.GetFromJsonAsync<List<Round>>(
                $"api/content/tournaments/{tournamentId}/rounds");

            return result ?? new List<Round>();
        }

        public async Task<Round> CreateRoundAsync(RoundDto newRound)
        {
            var response = await _http.PostAsJsonAsync("api/content/rounds", newRound);

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<Round>();
            return created!;
        }

    }
}
