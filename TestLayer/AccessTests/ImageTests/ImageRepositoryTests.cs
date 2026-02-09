using ImageDomain.ControlModule;
using ImageDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServerCommonModule.Database.Interfaces;
using ServerCommonModule.Repository;
using ServerCommonModule.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestLayer.ImageTests
{
    [TestClass]
    public class ImageRepositoryTests
    {
        private Mock<IDbUtilityFactory> dbFactoryMock;
        private Mock<IEnvironmentalParameters> envMock;
        private Mock<IRepositoryManager<Image>> repoManagerMock;
        private Mock<IRepositoryFactory> factoryMock;

        private ImageRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbFactoryMock = new Mock<IDbUtilityFactory>();
            envMock = new Mock<IEnvironmentalParameters>();
            repoManagerMock = new Mock<IRepositoryManager<Image>>();
            factoryMock = new Mock<IRepositoryFactory>();

            envMock.SetupGet(e => e.ConnectionString).Returns("Host=test;");
            envMock.SetupGet(e => e.DatabaseType).Returns("PostgreSQL");

            repository = new ImageRepository(envMock.Object, dbFactoryMock.Object);

            typeof(ImageRepository)
                .GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(repository, factoryMock.Object);
        }

        [TestMethod]
        public async Task GetImagesForObject_FiltersCorrectly()
        {
            var objId = Guid.NewGuid();

            var images = new ImageCollection
            {
                new Image { Id = Guid.NewGuid(), ObjectId = objId, ObjectType = 2 },
                new Image { Id = Guid.NewGuid(), ObjectId = Guid.NewGuid(), ObjectType = 2 }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<ImageCollection>()))
                .Callback<DataCollection<Image>>(dc =>
                {
                    foreach (var i in images)
                        dc.Add(i);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetImagesForObject(objId, 2);
            var list = result.ToList();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(objId, list[0].ObjectId);
        }

        [TestMethod]
        public async Task GetImageById_ReturnsCorrectImage()
        {
            var id = Guid.NewGuid();

            var images = new ImageCollection
            {
                new Image { Id = id, ObjectId = Guid.NewGuid(), ObjectType = 1 }
            };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<ImageCollection>()))
                .Callback<DataCollection<Image>>(dc =>
                {
                    foreach (var i in images)
                        dc.Add(i);
                })
                .Returns(repoManagerMock.Object);

            var result = await repository.GetImageById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }

        [TestMethod]
        public async Task CreateImage_InsertsImage()
        {
            var img = new Image { Id = Guid.NewGuid() };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);
            repoManagerMock.Setup(r => r.InsertSingleItem(img)).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<ImageCollection>()))
                .Returns(repoManagerMock.Object);

            var result = await repository.CreateImage(img);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.InsertSingleItem(img), Times.Once);
        }

        [TestMethod]
        public async Task UpdateImage_CallsUpdate()
        {
            var img = new Image { Id = Guid.NewGuid() };

            // LoadCollection must run so imageRepoManager is assigned
            repoManagerMock
                .Setup(r => r.LoadCollection())
                .Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<ImageCollection>()))
                .Returns(repoManagerMock.Object);

            repoManagerMock
                .Setup(r => r.UpdateSingleItem(img))
                .Returns(Task.CompletedTask);

            // Act
            var result = await repository.UpdateImage(img);

            // Assert
            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.UpdateSingleItem(img), Times.Once);
        }



        [TestMethod]
        public async Task DeleteImage_RemovesAndDeletes()
        {
            var img = new Image { Id = Guid.NewGuid() };
            var images = new ImageCollection { img };

            repoManagerMock.Setup(r => r.LoadCollection()).Returns(Task.CompletedTask);

            factoryMock
                .Setup(f => f.Get(It.IsAny<ImageCollection>()))
                .Callback<DataCollection<Image>>(dc =>
                {
                    foreach (var i in images)
                        dc.Add(i);
                })
                .Returns(repoManagerMock.Object);

            repoManagerMock.Setup(r => r.DeleteSingleItem(img)).Returns(Task.CompletedTask);

            var result = await repository.DeleteImage(img);

            Assert.AreEqual(string.Empty, result);
            repoManagerMock.Verify(r => r.DeleteSingleItem(img), Times.Once);
        }
    }
}
