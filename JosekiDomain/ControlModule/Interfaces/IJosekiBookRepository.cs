using JosekiDomain.Model;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.ControlModule.Interfaces
{
    public interface IJosekiBookRepository
    {
        Task<JosekiBookCollection> GetAllBooks(bool reload = true);
        Task<JosekiBook?> GetBookById(Guid id, bool reload = true);

        Task<JosekiBookCollection> SearchByTitle(string title, bool reload = true);

        Task<string> CreateBook(JosekiBook newBook, bool reload = true);
        Task<string> UpdateBook(JosekiBook updatedBook, bool reload = true);
        Task<string> DeleteBook(JosekiBook deleteBook, bool reload = true);
    }
}
