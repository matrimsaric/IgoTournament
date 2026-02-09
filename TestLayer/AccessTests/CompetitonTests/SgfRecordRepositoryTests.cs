using CompetitionDomain.ControlModule;
using CompetitionDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class SgfRecordRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<SgfRecord>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private SgfRecordRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<SgfRecord>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new SgfRecordRepository(envMock.Object, dbFactoryMock.Object);

            typeof(SgfRecordRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllSgfRecords_LoadsCollection()
        {
            var records = new SgfRecordCollection
            {
                new SgfRecord { Id = Guid.NewGuid(), MatchId = Guid.NewGuid() }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Callback<DataCollection<SgfRecord>>(dc =>
                {
                    foreach (var r in records)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetAllSgfRecords(true);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(records.First().MatchId, list[0].MatchId);
        }

        [TestMethod]
        public async Task GetSgfRecordById_ReturnsCorrectRecord()
        {
            var id = Guid.NewGuid();

            var records = new SgfRecordCollection
            {
                new SgfRecord { Id = id, MatchId = Guid.NewGuid() }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Callback<DataCollection<SgfRecord>>(dc =>
                {
                    foreach (var r in records)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetSgfRecordById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task GetSgfRecordByMatchId_ReturnsCorrectRecord()
        {
            var matchId = Guid.NewGuid();

            var records = new SgfRecordCollection
            {
                new SgfRecord { Id = Guid.NewGuid(), MatchId = matchId }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Callback<DataCollection<SgfRecord>>(dc =>
                {
                    foreach (var r in records)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetSgfRecordByMatchId(matchId);

            Assert.IsNotNull(result);
            Assert.AreEqual(matchId, result.MatchId);
        }

        [TestMethod]
        public async Task CreateSgfRecord_InsertsRecord()
        {
            var newRecord = new SgfRecord
            {
                Id = Guid.NewGuid(),
                MatchId = Guid.NewGuid()
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            repoManagerMock
                .Setup(r => r.InsertSingleItem(newRecord))
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreateSgfRecord(newRecord);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newRecord), Times.Once);
            Assert.AreNotEqual(default, newRecord.ModifiedDate);
        }

        [TestMethod]
        public async Task UpdateSgfRecord_CallsUpdate()
        {
            var existing = new SgfRecord { Id = Guid.NewGuid(), MatchId = Guid.NewGuid() };
            var records = new SgfRecordCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Callback<DataCollection<SgfRecord>>(dc =>
                {
                    foreach (var r in records)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.UpdateSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.UpdateSgfRecord(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
            Assert.AreNotEqual(default, existing.ModifiedDate);
        }

        [TestMethod]
        public async Task DeleteSgfRecord_RemovesAndDeletes()
        {
            var existing = new SgfRecord { Id = Guid.NewGuid(), MatchId = Guid.NewGuid() };
            var records = new SgfRecordCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<SgfRecordCollection>()))
                .Callback<DataCollection<SgfRecord>>(dc =>
                {
                    foreach (var r in records)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.DeleteSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.DeleteSgfRecord(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }
    }
}
