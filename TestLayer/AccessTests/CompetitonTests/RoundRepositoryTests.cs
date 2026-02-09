using CompetitionDomain.ControlModule;
using CompetitionDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class RoundRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<Round>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private RoundRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<Round>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new RoundRepository(envMock.Object, dbFactoryMock.Object);

            typeof(RoundRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllRounds_LoadsCollection()
        {
            var rounds = new RoundCollection
            {
                new Round { Id = Guid.NewGuid(), RoundNumber = 1 }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<RoundCollection>()))
                .Callback<DataCollection<Round>>(dc =>
                {
                    foreach (var r in rounds)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetAllRounds(true);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.ToList()[0].RoundNumber);

        }

        [TestMethod]
        public async Task GetRoundById_ReturnsCorrectRound()
        {
            var id = Guid.NewGuid();

            var rounds = new RoundCollection
            {
                new Round { Id = id, RoundNumber = 3 }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<RoundCollection>()))
                .Callback<DataCollection<Round>>(dc =>
                {
                    foreach (var r in rounds)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetRoundById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.RoundNumber);
        }

        [TestMethod]
        public async Task CreateRound_InsertsRound()
        {
            var newRound = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 5
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            repoManagerMock
                .Setup(r => r.InsertSingleItem(newRound))
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<RoundCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreateRound(newRound);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newRound), Times.Once);
            Assert.AreNotEqual(default, newRound.ModifiedDate);
        }

        [TestMethod]
        public async Task UpdateRound_CallsUpdate()
        {
            var existing = new Round { Id = Guid.NewGuid(), RoundNumber = 2 };
            var rounds = new RoundCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<RoundCollection>()))
                .Callback<DataCollection<Round>>(dc =>
                {
                    foreach (var r in rounds)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.UpdateSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.UpdateRound(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
            Assert.AreNotEqual(default, existing.ModifiedDate);
        }

        [TestMethod]
        public async Task DeleteRound_RemovesAndDeletes()
        {
            var existing = new Round { Id = Guid.NewGuid(), RoundNumber = 4 };
            var rounds = new RoundCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<RoundCollection>()))
                .Callback<DataCollection<Round>>(dc =>
                {
                    foreach (var r in rounds)
                        dc.Add(r);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.DeleteSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.DeleteRound(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }
    }
}
