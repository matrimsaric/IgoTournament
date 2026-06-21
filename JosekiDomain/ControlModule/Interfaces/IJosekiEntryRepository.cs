using JosekiDomain.Model;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.ControlModule.Interfaces
{
    public interface IJosekiEntryRepository
    {
        Task<JosekiEntryCollection> GetAllJoseki(bool reload = true);
        Task<JosekiEntry?> GetJosekiById(Guid id, bool reload = true);

        Task<JosekiEntryCollection> GetByCategory(int category, bool reload = true);
        Task<JosekiEntryCollection> GetChildren(Guid parentId, bool reload = true);
        Task<JosekiEntryCollection> GetByBook(Guid bookId, bool reload = true);
        Task<JosekiEntryCollection> GetRootJoseki(bool reload = true);

        Task<string> CreateJoseki(JosekiEntry newEntry, bool reload = true);
        Task<string> UpdateJoseki(JosekiEntry updatedEntry, bool reload = true);
        Task<string> DeleteJoseki(JosekiEntry deleteEntry, bool reload = true);
    }
}
