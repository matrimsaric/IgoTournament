using JosekiDomain.ControlModule.Interfaces;
using JosekiDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace JosekiDomain.ControlModule
{
    public class JosekiEntryRepository : IJosekiEntryRepository
    {
        private readonly IRepositoryFactory factory;
        private readonly IDbUtilityFactory dbUtilityFactory;
        private IRepositoryManager<JosekiEntry>? repoManager;

        private JosekiEntryCollection entries = new JosekiEntryCollection();

        public JosekiEntryRepository(IEnvironmentalParameters env, IDbUtilityFactory dbFactory)
        {
            dbUtilityFactory = dbFactory;
            factory = new RepositoryFactory(dbFactory, env);
        }

        private async Task<JosekiEntryCollection> LoadCollection(bool reload)
        {
            if (reload || entries.Count == 0)
            {
                entries = new JosekiEntryCollection();
                repoManager = factory.Get(entries);
                await repoManager.LoadCollection();
            }

            return entries;
        }

        public async Task<JosekiEntryCollection> GetAllJoseki(bool reload = true)
        {
            return await LoadCollection(reload);
        }

        public async Task<JosekiEntry?> GetJosekiById(Guid id, bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            return all.FindById(id);
        }

        public async Task<JosekiEntryCollection> GetByCategory(int category, bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            JosekiEntryCollection result = new JosekiEntryCollection();

            foreach (var item in all)
                if (item.Category == category)
                    result.Add(item);

            return result;
        }

        public async Task<JosekiEntryCollection> GetChildren(Guid parentId, bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            JosekiEntryCollection result = new JosekiEntryCollection();

            foreach (var item in all)
                if (item.ParentId == parentId)
                    result.Add(item);

            return result;
        }

        public async Task<JosekiEntryCollection> GetByBook(Guid bookId, bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            JosekiEntryCollection result = new JosekiEntryCollection();

            foreach (var item in all)
                if (item.BookId == bookId)
                    result.Add(item);

            return result;
        }

        public async Task<JosekiEntryCollection> GetRootJoseki(bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            JosekiEntryCollection result = new JosekiEntryCollection();

            foreach (var item in all)
                if (item.ParentId == null)
                    result.Add(item);

            return result;
        }

        public async Task<string> CreateJoseki(JosekiEntry newEntry, bool reload = true)
        {
            newEntry.ModifiedDate = DateTime.UtcNow;
            JosekiEntryCollection all = await LoadCollection(reload);
            all.Add(newEntry);

            await repoManager!.InsertSingleItem(newEntry);
            return string.Empty;
        }

        public async Task<string> UpdateJoseki(JosekiEntry updatedEntry, bool reload = true)
        {
            updatedEntry.ModifiedDate = DateTime.UtcNow;
            await LoadCollection(reload);

            await repoManager!.UpdateSingleItem(updatedEntry);
            return string.Empty;
        }

        public async Task<string> DeleteJoseki(JosekiEntry deleteEntry, bool reload = true)
        {
            JosekiEntryCollection all = await LoadCollection(reload);
            all.Remove(deleteEntry);

            await repoManager!.DeleteSingleItem(deleteEntry);
            return string.Empty;
        }
    }
}
