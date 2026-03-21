using CompetitionDomain.Model;
using StoneLedger.Services.Api.Interfaces;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class SgfService : ISgfService
    {
        private readonly HttpClient _http;

        public SgfService(HttpClient http)
        {
            _http = http;
        }

        public async Task<SgfRecord?> GetSgfRecordByIdAsync(Guid id)
        {
            var sgf =  await _http.GetFromJsonAsync<SgfRecord>($"api/content/sgf-records/{id}");
            return sgf;
        }

        public async Task<SgfRecord?> GetSgfRecordByMatchIdAsync(Guid matchId)
        {
            return await _http.GetFromJsonAsync<SgfRecord>($"api/content/sgf-records/by-match/{matchId}");
        }

        public async Task CreateSgfRecord(SgfRecord newSgfRecord)
        {
            var response = await _http.PostAsJsonAsync("api/content/sgf-records", newSgfRecord);
            response.EnsureSuccessStatusCode();
        }
    }
}
