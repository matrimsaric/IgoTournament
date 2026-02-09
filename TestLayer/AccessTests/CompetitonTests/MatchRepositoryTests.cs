using CompetitionDomain.ControlModule;
using CompetitionDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using Match = CompetitionDomain.Model.Match;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class MatchRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<Match>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private MatchRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<Match>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new MatchRepository(envMock.Object, dbFactoryMock.Object);

            // Inject the mocked factory into the private field
            typeof(MatchRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllMatches_LoadsCollection()
        {
            var matches = new MatchCollection
            {
                new Match { Id = Guid.NewGuid(), Name = "Board 1" }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
     .Setup(f => f.Get(It.IsAny<MatchCollection>()))
     .Callback<DataCollection<Match>>(dc =>
     {
         foreach (var m in matches)
             dc.Add(m);
     })
     .Returns(repoManagerMock.Object);


            var result = await repository.GetAllMatches(true);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Board 1", result.FirstOrDefault()?.Name);
        }

        [TestMethod]
        public async Task GetMatchById_ReturnsCorrectMatch()
        {
            var id = Guid.NewGuid();

            var matches = new MatchCollection
            {
                new Match { Id = id, Name = "Round 3 Board 2" }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
    .Setup(f => f.Get(It.IsAny<MatchCollection>()))
    .Callback<DataCollection<Match>>(dc =>
    {
        foreach (var m in matches)
            dc.Add(m);
    })
    .Returns(repoManagerMock.Object);



            var result = await repository.GetMatchById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Round 3 Board 2", result.Name);
        }

        [TestMethod]
        public async Task CreateMatch_InsertsMatch()
        {
            var newMatch = new Match
            {
                Id = Guid.NewGuid(),
                Name = "New Match"
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            repoManagerMock
                .Setup(r => r.InsertSingleItem(newMatch))
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<MatchCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreateMatch(newMatch);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newMatch), Times.Once);
            Assert.AreNotEqual(default, newMatch.ModifiedDate);
        }

        [TestMethod]
        public async Task UpdateMatch_CallsUpdate()
        {
            var existing = new Match { Id = Guid.NewGuid(), Name = "Update Me" };
            var matches = new MatchCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
    .Setup(f => f.Get(It.IsAny<MatchCollection>()))
    .Callback<DataCollection<Match>>(dc =>
    {
        foreach (var m in matches)
            dc.Add(m);
    })
    .Returns(repoManagerMock.Object);


            repoManagerMock
                .Setup(r => r.UpdateSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.UpdateMatch(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
            Assert.AreNotEqual(default, existing.ModifiedDate);
        }

        [TestMethod]
        public async Task DeleteMatch_RemovesAndDeletes()
        {
            var existing = new Match { Id = Guid.NewGuid(), Name = "Delete Me" };
            var matches = new MatchCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
    .Setup(f => f.Get(It.IsAny<MatchCollection>()))
    .Callback<DataCollection<Match>>(dc =>
    {
        foreach (var m in matches)
            dc.Add(m);
    })
    .Returns(repoManagerMock.Object);


            repoManagerMock
                .Setup(r => r.DeleteSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.DeleteMatch(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }
    }
}
