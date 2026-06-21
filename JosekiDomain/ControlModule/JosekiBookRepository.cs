using JosekiDomain.ControlModule.Interfaces;
using JosekiDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.ControlModule
{
    public class JosekiBookRepository : IJosekiBookRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<JosekiBook>? repoManager;

        private JosekiBookCollection books = new JosekiBookCollection();

        public JosekiBookRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<JosekiBookCollection> LoadCollection(bool reload)
        {
            if (reload || books.Count == 0)
            {
                books = new JosekiBookCollection();
                repoManager = factory.Get(books);
                await repoManager.LoadCollection();
            }

            return books;
        }

        public async Task<JosekiBookCollection> GetAllBooks(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<JosekiBook?> GetBookById(Guid id, bool reload = true)
        {
            JosekiBookCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<JosekiBookCollection> SearchByTitle(string title, bool reload = true)
        {
            JosekiBookCollection all = await LoadCollection(reload);
            JosekiBookCollection result = new JosekiBookCollection();

            foreach (var item in all)
                if (item.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                    result.Add(item);

            return result;
        }

        public async Task<string> CreateBook(JosekiBook newBook, bool reload = true)
        {
            newBook.ModifiedDate = DateTime.UtcNow;
            JosekiBookCollection all = await LoadCollection(reload);
            all.Add(newBook);

            await repoManager!.InsertSingleItem(newBook);
            return string.Empty;
        }

        public async Task<string> UpdateBook(JosekiBook updatedBook, bool reload = true)
        {
            updatedBook.ModifiedDate = DateTime.UtcNow;
            await LoadCollection(reload);

            await repoManager!.UpdateSingleItem(updatedBook);
            return string.Empty;
        }

        public async Task<string> DeleteBook(JosekiBook deleteBook, bool reload = true)
        {
            JosekiBookCollection all = await LoadCollection(reload);
            all.Remove(deleteBook);

            await repoManager!.DeleteSingleItem(deleteBook);
            return string.Empty;
        }
    }
}
