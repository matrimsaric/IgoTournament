using JosekiDomain.Model;

namespace StoneLedger.Services.Api
{
    public interface IJosekiEntryService
    {
        Task<List<JosekiEntry>> GetAllJosekiAsync();
        Task<JosekiEntry?> GetJosekiByIdAsync(Guid id);

        Task<List<JosekiEntry>> GetByCategoryAsync(int category);
        Task<List<JosekiEntry>> GetChildrenAsync(Guid parentId);
        Task<List<JosekiEntry>> GetRootJosekiAsync();
        Task<List<JosekiEntry>> GetByBookAsync(Guid bookId);

        Task<JosekiEntry> CreateJosekiAsync(JosekiEntry newEntry);
        Task<string> UpdateJosekiAsync(JosekiEntry updatedEntry);
        Task<string> DeleteJosekiAsync(Guid id);
    }
}
