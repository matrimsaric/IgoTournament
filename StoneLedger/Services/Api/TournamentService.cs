using CompetitionDomain.Model;
using StoneLedger.Models;
using StoneLedger.Services.Api.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class TournamentService : ITournamentService
    {
        private readonly HttpClient _http;

        public TournamentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Tournament>> GetAllTournamentsAsync()
        {
            var response = await _http.GetAsync("api/content/tournaments");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[TournamentService] GetAllTournamentsAsync() FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return new List<Tournament>();
            }

            var result = await response.Content.ReadFromJsonAsync<List<Tournament>>() ?? new List<Tournament>();

            return result.Select(t => new Tournament
            {
                Id = t.Id,
                Name = t.Name,
            }).ToList();
        }
    }
}
