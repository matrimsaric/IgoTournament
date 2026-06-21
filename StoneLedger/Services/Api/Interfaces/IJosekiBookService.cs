using JosekiDomain.Model;

namespace StoneLedger.Services.Api
{
    public interface IJosekiBookService
    {
        Task<List<JosekiBook>> GetAllBooksAsync();
        Task<JosekiBook?> GetBookByIdAsync(Guid id);

        Task<List<JosekiBook>> SearchBooksAsync(string title);

        Task<JosekiBook> CreateBookAsync(JosekiBook newBook);
        Task<string> UpdateBookAsync(JosekiBook updatedBook);
        Task<string> DeleteBookAsync(Guid id);
    }
}
