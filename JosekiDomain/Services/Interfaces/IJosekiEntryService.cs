using JosekiDomain.Model;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.Services.Interfaces
{
    public interface IJosekiEntryService
    {
        Task<JosekiEntryCollection> GetAllJosekiAsync();
        Task<JosekiEntry?> GetJosekiByIdAsync(Guid id);

        Task<JosekiEntryCollection> GetByCategoryAsync(int category);
        Task<JosekiEntryCollection> GetChildrenAsync(Guid parentId);
        Task<JosekiEntryCollection> GetRootJosekiAsync();
        Task<JosekiEntryCollection> GetByBookAsync(Guid bookId);

        Task<string> CreateJosekiAsync(JosekiEntry newEntry);
        Task<string> UpdateJosekiAsync(JosekiEntry updatedEntry);
        Task<string> DeleteJosekiAsync(Guid entryId);
    }
}
