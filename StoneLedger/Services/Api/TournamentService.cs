using System.Net.Http.Json;
using CompetitionDomain.Model;

namespace StoneLedger.Services.Api
{
    public class TournamentService
    {
        private readonly HttpClient _http;

        public TournamentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Tournament>> GetAllTournamentsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<Tournament>>(
                "api/content/tournaments");

            return result ?? new List<Tournament>();
        }
    }
}
