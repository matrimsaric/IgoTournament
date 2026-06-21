using JosekiDomain.ControlModule.Interfaces;
using JosekiDomain.Model;
using JosekiDomain.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.Services
{
    public class JosekiBookService : IJosekiBookService
    {
        private readonly IJosekiBookRepository _repo;

        public JosekiBookService(IJosekiBookRepository repo)
        {
            _repo = repo;
        }

        public Task<JosekiBookCollection> GetAllBooksAsync()
            => _repo.GetAllBooks();

        public Task<JosekiBook?> GetBookByIdAsync(Guid id)
            => _repo.GetBookById(id);

        public Task<JosekiBookCollection> SearchByTitleAsync(string title)
            => _repo.SearchByTitle(title);

        public Task<string> CreateBookAsync(JosekiBook newBook)
            => _repo.CreateBook(newBook);

        public Task<string> UpdateBookAsync(JosekiBook updatedBook)
            => _repo.UpdateBook(updatedBook);

        public Task<string> DeleteBookAsync(Guid bookId)
            => _repo.DeleteBook(new JosekiBook { Id = bookId });
    }
}
