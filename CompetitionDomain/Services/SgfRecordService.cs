using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using CompetitionDomain.Services.Interfaces;

namespace CompetitionDomain.Services
{
    public class SgfRecordService : ISgfRecordService
    {
        private readonly ISgfRecordRepository _repo;

        public SgfRecordService(ISgfRecordRepository repo)
        {
            _repo = repo;
        }

        public Task<SgfRecordCollection> GetAllSgfRecordsAsync()
            => _repo.GetAllSgfRecords();

        public Task<SgfRecord?> GetSgfRecordByIdAsync(Guid id)
            => _repo.GetSgfRecordById(id);

        public Task<SgfRecord?> GetSgfRecordByMatchIdAsync(Guid matchId)
            => _repo.GetSgfRecordByMatchId(matchId);

        public Task<string> CreateSgfRecordAsync(SgfRecord newRecord)
            => _repo.CreateSgfRecord(newRecord);

        public Task<string> UpdateSgfRecordAsync(SgfRecord updatedRecord)
            => _repo.UpdateSgfRecord(updatedRecord);

        public Task<string> DeleteSgfRecordAsync(Guid recordId)
            => _repo.DeleteSgfRecord(new SgfRecord { Id = recordId });
    }
}
