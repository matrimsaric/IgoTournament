using CompetitionDomain.Model;
using System;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface ISgfRecordRepository
    {
        Task<SgfRecordCollection> GetAllSgfRecords(bool reload = true);
        Task<SgfRecord?> GetSgfRecordById(Guid id, bool reload = true);
        Task<SgfRecord?> GetSgfRecordByMatchId(Guid matchId, bool reload = true);

        Task<string> CreateSgfRecord(SgfRecord newRecord, bool reload = true);
        Task<string> UpdateSgfRecord(SgfRecord updatedRecord, bool reload = true);
        Task<string> DeleteSgfRecord(SgfRecord deleteRecord, bool reload = true);
    }
}
