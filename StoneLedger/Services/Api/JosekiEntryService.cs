using JosekiDomain.Model;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class JosekiEntryService : IJosekiEntryService
    {
        private readonly HttpClient _http;

        public JosekiEntryService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<JosekiEntry>> GetAllJosekiAsync()
        {
            var result = await _http.GetFromJsonAsync<List<JosekiEntry>>(
                "api/content/joseki");

            return result ?? new List<JosekiEntry>();
        }

        public async Task<JosekiEntry?> GetJosekiByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<JosekiEntry>(
                $"api/content/joseki/{id}");
        }

        public async Task<List<JosekiEntry>> GetByCategoryAsync(int category)
        {
            var result = await _http.GetFromJsonAsync<List<JosekiEntry>>(
                $"api/content/joseki/category/{category}");

            return result ?? new List<JosekiEntry>();
        }

        public async Task<List<JosekiEntry>> GetChildrenAsync(Guid parentId)
        {
            var result = await _http.GetFromJsonAsync<List<JosekiEntry>>(
                $"api/content/joseki/{parentId}/children");

            return result ?? new List<JosekiEntry>();
        }

        public async Task<List<JosekiEntry>> GetRootJosekiAsync()
        {
            var result = await _http.GetFromJsonAsync<List<JosekiEntry>>(
                "api/content/joseki/roots");

            return result ?? new List<JosekiEntry>();
        }

        public async Task<List<JosekiEntry>> GetByBookAsync(Guid bookId)
        {
            var result = await _http.GetFromJsonAsync<List<JosekiEntry>>(
                $"api/content/joseki/book/{bookId}");

            return result ?? new List<JosekiEntry>();
        }

        public async Task<JosekiEntry> CreateJosekiAsync(JosekiEntry newEntry)
        {
            var response = await _http.PostAsJsonAsync(
                "api/content/joseki", newEntry);

            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<JosekiEntry>();
            return created!;
        }

        public async Task<string> UpdateJosekiAsync(JosekiEntry updatedEntry)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/content/joseki/{updatedEntry.Id}", updatedEntry);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteJosekiAsync(Guid id)
        {
            var response = await _http.DeleteAsync(
                $"api/content/joseki/{id}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
