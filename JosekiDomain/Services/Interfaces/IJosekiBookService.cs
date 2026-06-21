using JosekiDomain.Model;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.Services.Interfaces
{
    public interface IJosekiBookService
    {
        Task<JosekiBookCollection> GetAllBooksAsync();
        Task<JosekiBook?> GetBookByIdAsync(Guid id);

        Task<JosekiBookCollection> SearchByTitleAsync(string title);

        Task<string> CreateBookAsync(JosekiBook newBook);
        Task<string> UpdateBookAsync(JosekiBook updatedBook);
        Task<string> DeleteBookAsync(Guid bookId);
    }
}
