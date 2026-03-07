using CompetitionDomain.Model;
using StoneLedger.Models;
using StoneLedger.Services.Api.Interfaces;
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
            var result = await _http.GetFromJsonAsync<List<Tournament>>(
            "api/content/tournaments") ?? new List<Tournament>();

            return result.Select(t => new Tournament
            {
                Id = t.Id,
                Name = t.Name,
            }).ToList();
        }
    }
}
