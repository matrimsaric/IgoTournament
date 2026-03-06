using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.Services.Interfaces
{
    public interface ISgfRecordService
    {
        Task<SgfRecordCollection> GetAllSgfRecordsAsync();
        Task<SgfRecord?> GetSgfRecordByIdAsync(Guid id);
        Task<SgfRecord?> GetSgfRecordByMatchIdAsync(Guid matchId);

        Task<string> CreateSgfRecordAsync(SgfRecord newRecord);
        Task<string> UpdateSgfRecordAsync(SgfRecord updatedRecord);
        Task<string> DeleteSgfRecordAsync(Guid recordId);
    }
}
