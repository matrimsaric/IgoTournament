using JosekiDomain.Model;
using System.Net.Http.Json;

namespace StoneLedger.Services.Api
{
    public class JosekiBookService : IJosekiBookService
    {
        private readonly HttpClient _http;

        public JosekiBookService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<JosekiBook>> GetAllBooksAsync()
        {
            var result = await _http.GetFromJsonAsync<List<JosekiBook>>(
                "api/content/joseki/books");

            return result ?? new List<JosekiBook>();
        }

        public async Task<JosekiBook?> GetBookByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<JosekiBook>(
                $"api/content/joseki/books/{id}");
        }

        public async Task<List<JosekiBook>> SearchBooksAsync(string title)
        {
            var result = await _http.GetFromJsonAsync<List<JosekiBook>>(
                $"api/content/joseki/books/search/{title}");

            return result ?? new List<JosekiBook>();
        }

        public async Task<JosekiBook> CreateBookAsync(JosekiBook newBook)
        {
            var response = await _http.PostAsJsonAsync(
                "api/content/joseki/books", newBook);

            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<JosekiBook>();
            return created!;
        }

        public async Task<string> UpdateBookAsync(JosekiBook updatedBook)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/content/joseki/books/{updatedBook.Id}", updatedBook);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteBookAsync(Guid id)
        {
            var response = await _http.DeleteAsync(
                $"api/content/joseki/books/{id}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
