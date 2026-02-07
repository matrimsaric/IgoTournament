using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayerDomain.ControlModule;
using PlayerDomain.Model;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace TestLayer.PlayerTests
{
    [TestClass]
    public class PlayerRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<Player>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private PlayerRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<Player>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new PlayerRepository(envMock.Object, dbFactoryMock.Object);

            typeof(PlayerRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllPlayers_LoadsCollection()
        {
            // Arrange
            var players = new PlayerCollection
    {
        new Player { Id = Guid.NewGuid(), Name = "Lee Sedol", Rank = "9" }
    };

            // LoadCollection() must be allowed to run
            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            // Factory must return the repo manager AND populate the collection
            factoryMock
                .Setup(f => f.Get(It.IsAny<DataCollection<Player>>()))
                .Callback<DataCollection<Player>>(pc =>
                {
                    foreach (var p in players)
                        pc.Add(p);
                })
                .Returns(repoManagerMock.Object);

            // Act
            var result = await repository.GetAllPlayers(true);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Lee Sedol", result?.FirstOrDefault().Name);
        }


        [TestMethod]
        public async Task GetPlayerById_ReturnsCorrectPlayer()
        {
            // Arrange
            var id = Guid.NewGuid();

            var players = new PlayerCollection
            {
                new Player { Id = id, Name = "Iyama Yuta", Rank = "9" }
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<DataCollection<Player>>()))
                .Callback<DataCollection<Player>>(pc =>
                {
                    foreach (var p in players)
                        pc.Add(p);
                })
                .Returns(repoManagerMock.Object);

            // Act
            var result = await repository.GetPlayerById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Iyama Yuta", result.Name);
        }


        [TestMethod]
        public async Task CreatePlayer_Inserts_WhenNoDuplicate()
        {
            var newPlayer = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Shin Jinseo",
                Rank = "9"
            };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            repoManagerMock
                .Setup(r => r.InsertSingleItem(newPlayer))
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<PlayerCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreatePlayer(newPlayer);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newPlayer), Times.Once);
        }

        [TestMethod]
        public async Task CreatePlayer_ReturnsError_WhenDuplicate()
        {
            var existing = new Player { Id = Guid.NewGuid(), Name = "Lee Sedol", Rank = "9" };
            var newPlayer = new Player { Id = Guid.NewGuid(), Name = "Lee Sedol", Rank = "1" };

            var players = new PlayerCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<PlayerCollection>()))
               .Callback<DataCollection<Player>>(pc =>
               {
                   foreach (var p in players)
                       pc.Add(p);
               })
               .Returns(repoManagerMock.Object);

            var result = await repository.CreatePlayer(newPlayer);

            Assert.AreEqual("Duplicate player detected.", result);
            repoManagerMock.Verify(r => r.InsertSingleItem(It.IsAny<Player>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdatePlayer_CallsUpdate()
        {
            var existing = new Player { Id = Guid.NewGuid(), Name = "Ke Jie", Rank = "9" };
            var players = new PlayerCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<PlayerCollection>()))
                 .Callback<DataCollection<Player>>(pc =>
                 {
                     foreach (var p in players)
                         pc.Add(p);
                 })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.UpdateSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.UpdatePlayer(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
        }

        [TestMethod]
        public async Task DeletePlayer_RemovesAndDeletes()
        {
            var existing = new Player { Id = Guid.NewGuid(), Name = "Iyama Yuta", Rank = "9" };
            var players = new PlayerCollection { existing };

            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<PlayerCollection>()))
                 .Callback<DataCollection<Player>>(pc =>
                 {
                     foreach (var p in players)
                         pc.Add(p);
                 })
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.DeleteSingleItem(existing))
                .Returns(Task.CompletedTask);

            var result = await repository.DeletePlayer(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }
    }
}
