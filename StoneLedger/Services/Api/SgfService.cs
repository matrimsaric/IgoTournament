using CompetitionDomain.Model;
using StoneLedger.Services.Api.Interfaces;
using System.Diagnostics;
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
            var response = await _http.GetAsync($"api/content/sgf-records/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[SgfService] GetSgfRecordByIdAsync({id}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<SgfRecord>();
        }

        public async Task<SgfRecord?> GetSgfRecordByMatchIdAsync(Guid matchId)
        {
            var response = await _http.GetAsync($"api/content/sgf-records/by-match/{matchId}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[SgfService] GetSgfRecordByMatchIdAsync({matchId}) FAILED status={(int)response.StatusCode} {response.StatusCode} body={body}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<SgfRecord>();
        }

        public async Task CreateSgfRecord(SgfRecord newSgfRecord)
        {
            var response = await _http.PostAsJsonAsync("api/content/sgf-records", newSgfRecord);
            response.EnsureSuccessStatusCode();
        }
    }
}
