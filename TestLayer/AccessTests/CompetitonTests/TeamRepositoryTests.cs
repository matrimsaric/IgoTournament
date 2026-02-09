using CommonModule.Enums;
using CompetitionDomain.ControlModule;
using CompetitionDomain.Model;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
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
    public class TeamRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<Team>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;
        private Mock<IImageService> imageServiceMock;

        private TeamRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<Team>>();
            factoryMock = new Mock<IRepositoryFactory>();
            imageServiceMock = new Mock<IImageService>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new TeamRepository(envMock.Object, dbFactoryMock.Object);

            typeof(TeamRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);

            typeof(TeamRepository)
                .GetField("imageService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, imageServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAllTeams_LoadsCollection()
        {
            var teams = new TeamCollection
            {
                new Team { Id = Guid.NewGuid(), Name = "Alpha" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Callback<DataCollection<Team>>(dc =>
                {
                    foreach (var t in teams)
                        dc.Add(t);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetAllTeams(true);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Alpha", list[0].Name);
        }

        [TestMethod]
        public async Task GetTeamById_ReturnsCorrectTeam()
        {
            var id = Guid.NewGuid();

            var teams = new TeamCollection
            {
                new Team { Id = id, Name = "Bravo" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Callback<DataCollection<Team>>(dc =>
                {
                    foreach (var t in teams)
                        dc.Add(t);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetTeamById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Bravo", result.Name);
        }

        [TestMethod]
        public async Task CreateTeam_InsertsTeam()
        {
            var newTeam = new Team
            {
                Id = Guid.NewGuid(),
                Name = "New Team"
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);
            repoManagerMock.Setup(r => r.InsertSingleItem(newTeam)).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreateTeam(newTeam);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(newTeam), Times.Once);
        }

        [TestMethod]
        public async Task CreateTeam_DetectsDuplicate()
        {
            var teams = new TeamCollection
            {
                new Team { Id = Guid.NewGuid(), Name = "Alpha" }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Callback<DataCollection<Team>>(dc =>
                {
                    foreach (var t in teams)
                        dc.Add(t);
                })
                .Returns(repoManagerMock.Object);

            var duplicate = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Alpha"
            };

            var result = await repository.CreateTeam(duplicate);

            Assert.AreEqual("Duplicate team detected.", result);
            repoManagerMock.Verify(r => r.InsertSingleItem(It.IsAny<Team>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateTeam_CallsUpdate()
        {
            var existing = new Team { Id = Guid.NewGuid(), Name = "Update Me" };
            var teams = new TeamCollection { existing };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Callback<DataCollection<Team>>(dc =>
                {
                    foreach (var t in teams)
                        dc.Add(t);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock.Setup(r => r.UpdateSingleItem(existing)).Returns(Task.CompletedTask);

            var result = await repository.UpdateTeam(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(existing), Times.Once);
        }

        [TestMethod]
        public async Task DeleteTeam_RemovesAndDeletes()
        {
            var existing = new Team { Id = Guid.NewGuid(), Name = "Delete Me" };
            var teams = new TeamCollection { existing };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<TeamCollection>()))
                .Callback<DataCollection<Team>>(dc =>
                {
                    foreach (var t in teams)
                        dc.Add(t);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock.Setup(r => r.DeleteSingleItem(existing)).Returns(Task.CompletedTask);

            var result = await repository.DeleteTeam(existing);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(existing), Times.Once);
        }

        // ------------------------------------------------------------
        // IMAGE SERVICE TESTS
        // ------------------------------------------------------------

        [TestMethod]
        public async Task GetImages_DelegatesToImageService()
        {
            var teamId = Guid.NewGuid();
            var images = new ImageCollection();

            imageServiceMock
                .Setup(s => s.GetImagesForObject(teamId, (int)ImageObjectType.Team, true))
                .ReturnsAsync(images);

            var result = await repository.GetImages(teamId);

            Assert.AreSame(images, result);
        }

        [TestMethod]
        public async Task GetImage_DelegatesToImageService()
        {
            var imageId = Guid.NewGuid();
            var image = new Image();

            imageServiceMock
                .Setup(s => s.GetImageById(imageId, true))
                .ReturnsAsync(image);

            var result = await repository.GetImage(imageId);

            Assert.AreSame(image, result);
        }

        [TestMethod]
        public async Task GetPrimaryImageForTeam_DelegatesToImageService()
        {
            var teamId = Guid.NewGuid();
            var image = new Image();

            imageServiceMock
                .Setup(s => s.GetPrimaryImageForObject(teamId, (int)ImageObjectType.Team, true))
                .ReturnsAsync(image);

            var result = await repository.GetPrimaryImageForTeam(teamId);

            Assert.AreSame(image, result);
        }

        [TestMethod]
        public async Task AddImage_DelegatesToImageService()
        {
            var teamId = Guid.NewGuid();
            var image = new Image();

            imageServiceMock
                .Setup(s => s.AddImage(image, true))
                .ReturnsAsync(string.Empty);

            var result = await repository.AddImage(teamId, image);

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual(teamId, image.ObjectId);
            Assert.AreEqual((int)ImageObjectType.Team, image.ObjectType);
        }

        [TestMethod]
        public async Task UpdateImage_DelegatesToImageService()
        {
            var image = new Image();

            imageServiceMock
                .Setup(s => s.UpdateImage(image, true))
                .ReturnsAsync(string.Empty);

            var result = await repository.UpdateImage(image);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public async Task DeleteImage_DelegatesToImageService()
        {
            var image = new Image();

            imageServiceMock
                .Setup(s => s.DeleteImage(image, true))
                .ReturnsAsync(string.Empty);

            var result = await repository.DeleteImage(image);

            Assert.AreEqual(string.Empty, result);
        }
    }
}
