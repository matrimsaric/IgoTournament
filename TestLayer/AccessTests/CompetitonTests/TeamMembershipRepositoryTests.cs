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
    public class TeamMembershipRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<TeamMembership>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private TeamMembershipRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<TeamMembership>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new TeamMembershipRepository(envMock.Object, dbFactoryMock.Object);

            typeof(TeamMembershipRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllMemberships_LoadsCollection()
        {
            var memberships = new TeamMembershipCollection
            {
                new TeamMembership { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), TeamId = Guid.NewGuid(), Season = "2024" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetAllMemberships(true);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("2024", list[0].Season);
        }

        [TestMethod]
        public async Task GetMembershipById_ReturnsCorrectMembership()
        {
            var id = Guid.NewGuid();

            var memberships = new TeamMembershipCollection
            {
                new TeamMembership { Id = id, PlayerId = Guid.NewGuid(), TeamId = Guid.NewGuid(), Season = "2023" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetMembershipById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task AddMembership_InsertsMembership()
        {
            var newMembership = new TeamMembership
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                TeamId = Guid.NewGuid(),
                Season = "2024"
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);
            repoManagerMock.Setup(r => r.InsertSingleItem(newMembership)).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.AddMembership(newMembership);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newMembership), Times.Once);
        }

        [TestMethod]
        public async Task AddMembership_DetectsDuplicate()
        {
            var player = Guid.NewGuid();
            var team = Guid.NewGuid();

            var existing = new TeamMembership
            {
                Id = Guid.NewGuid(),
                PlayerId = player,
                TeamId = team,
                Season = "2024"
            };

            var memberships = new TeamMembershipCollection { existing };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var duplicate = new TeamMembership
            {
                Id = Guid.NewGuid(),
                PlayerId = player,
                TeamId = team,
                Season = "2024"
            };

            var result = await repository.AddMembership(duplicate);

            Assert.AreEqual("Duplicate membership detected.", result);
            repoManagerMock.Verify(r => r.InsertSingleItem(It.IsAny<TeamMembership>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateMembership_CallsUpdate()
        {
            var existing = new TeamMembership
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                TeamId = Guid.NewGuid(),
                Season = "2023"
            };

            var memberships = new TeamMembershipCollection { existing };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock.Setup(r => r.UpdateSingleItem(existing)).Returns(Task.CompletedTask);

            var result = await repository.UpdateMembership(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
        }

        [TestMethod]
        public async Task RemoveMembership_RemovesAndDeletes()
        {
            var existing = new TeamMembership
            {
                Id = Guid.NewGuid(),
                PlayerId = Guid.NewGuid(),
                TeamId = Guid.NewGuid(),
                Season = "2024"
            };

            var memberships = new TeamMembershipCollection { existing };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock.Setup(r => r.DeleteSingleItem(existing)).Returns(Task.CompletedTask);

            var result = await repository.RemoveMembership(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }

        [TestMethod]
        public async Task GetMembershipsForTeam_FiltersCorrectly()
        {
            var team = Guid.NewGuid();

            var memberships = new TeamMembershipCollection
            {
                new TeamMembership { Id = Guid.NewGuid(), TeamId = team, PlayerId = Guid.NewGuid(), Season = "2024" },
                new TeamMembership { Id = Guid.NewGuid(), TeamId = Guid.NewGuid(), PlayerId = Guid.NewGuid(), Season = "2024" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetMembershipsForTeam(team);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(team, list[0].TeamId);
        }

        [TestMethod]
        public async Task GetMembershipsForPlayer_FiltersCorrectly()
        {
            var player = Guid.NewGuid();

            var memberships = new TeamMembershipCollection
            {
                new TeamMembership { Id = Guid.NewGuid(), PlayerId = player, TeamId = Guid.NewGuid(), Season = "2024" },
                new TeamMembership { Id = Guid.NewGuid(), PlayerId = Guid.NewGuid(), TeamId = Guid.NewGuid(), Season = "2024" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetMembershipsForPlayer(player);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(player, list[0].PlayerId);
        }

        [TestMethod]
        public async Task GetMembershipsForSeason_FiltersCorrectly()
        {
            var memberships = new TeamMembershipCollection
            {
                new TeamMembership { Id = Guid.NewGuid(), Season = "2024", PlayerId = Guid.NewGuid(), TeamId = Guid.NewGuid() },
                new TeamMembership { Id = Guid.NewGuid(), Season = "2023", PlayerId = Guid.NewGuid(), TeamId = Guid.NewGuid() }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamMembershipCollection>()))
                .Callback<DataCollection<TeamMembership>>(dc =>
                {
                    foreach (var m in memberships)
                        dc.Add(m);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetMembershipsForSeason("2024");
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("2024", list[0].Season);
        }
    }
}
