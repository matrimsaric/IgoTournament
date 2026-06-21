using JosekiDomain.ControlModule.Interfaces;
using JosekiDomain.Model;
using JosekiDomain.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.Services
{
    public class JosekiEntryService : IJosekiEntryService
    {
        private readonly IJosekiEntryRepository _repo;

        public JosekiEntryService(IJosekiEntryRepository repo)
        {
            _repo = repo;
        }

        public Task<JosekiEntryCollection> GetAllJosekiAsync()
            => _repo.GetAllJoseki();

        public Task<JosekiEntry?> GetJosekiByIdAsync(Guid id)
            => _repo.GetJosekiById(id);

        public Task<JosekiEntryCollection> GetByCategoryAsync(int category)
            => _repo.GetByCategory(category);

        public Task<JosekiEntryCollection> GetChildrenAsync(Guid parentId)
            => _repo.GetChildren(parentId);

        public Task<JosekiEntryCollection> GetRootJosekiAsync()
            => _repo.GetRootJoseki();

        public Task<JosekiEntryCollection> GetByBookAsync(Guid bookId)
            => _repo.GetByBook(bookId);

        public Task<string> CreateJosekiAsync(JosekiEntry newEntry)
            => _repo.CreateJoseki(newEntry);

        public Task<string> UpdateJosekiAsync(JosekiEntry updatedEntry)
            => _repo.UpdateJoseki(updatedEntry);

        public Task<string> DeleteJosekiAsync(Guid entryId)
            => _repo.DeleteJoseki(new JosekiEntry { Id = entryId });
    }
}
