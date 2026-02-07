using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionDomain.ControlModule
{
    public class SgfRecordRepository : ISgfRecordRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<SgfRecord>? sgfRepoManager;

        private SgfRecordCollection records = new SgfRecordCollection();

        public SgfRecordRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<SgfRecordCollection> LoadCollection(bool reload)
        {
            if (reload || records.Count == 0)
            {
                records = new SgfRecordCollection();
                sgfRepoManager = factory.Get(records);
                await sgfRepoManager.LoadCollection();
            }

            return records;
        }

        public async Task<SgfRecordCollection> GetAllSgfRecords(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<SgfRecord?> GetSgfRecordById(Guid id, bool reload = true)
        {
            var all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<SgfRecord?> GetSgfRecordByMatchId(Guid matchId, bool reload = true)
        {
            var all = await LoadCollection(reload);
            return all.FirstOrDefault(r => r.MatchId == matchId);
        }

        public async Task<string> CreateSgfRecord(SgfRecord newRecord, bool reload = true)
        {
            newRecord.ModifiedDate = DateTime.UtcNow;
            var all = await LoadCollection(reload);
            all.Add(newRecord);

            await sgfRepoManager!.InsertSingleItem(newRecord);
            return string.Empty;
        }

        public async Task<string> UpdateSgfRecord(SgfRecord updatedRecord, bool reload = true)
        {
            updatedRecord.ModifiedDate = DateTime.UtcNow;
            await LoadCollection(reload);
            await sgfRepoManager!.UpdateSingleItem(updatedRecord);
            return string.Empty;
        }

        public async Task<string> DeleteSgfRecord(SgfRecord deleteRecord, bool reload = true)
        {
            var all = await LoadCollection(reload);
            all.Remove(deleteRecord);

            await sgfRepoManager!.DeleteSingleItem(deleteRecord);
            return string.Empty;
        }
    }
}
