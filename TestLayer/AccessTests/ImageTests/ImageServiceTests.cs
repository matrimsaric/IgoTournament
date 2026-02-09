using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ImageDomain.ControlModule;
using ImageDomain.ControlModule.Interfaces;
using ImageDomain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestLayer.ImageTests
{
    [TestClass]
    public class ImageServiceTests
    {
        private Mock<IImageRepository> repoMock;
        private ImageService service;

        [TestInitialize]
        public void Setup()
        {
            repoMock = new Mock<IImageRepository>();
            service = new ImageService(repoMock.Object);
        }

        [TestMethod]
        public async Task GetImagesForObject_DelegatesToRepository()
        {
            var objId = Guid.NewGuid();
            var images = new ImageCollection();

            repoMock
                .Setup(r => r.GetImagesForObject(objId, 2, true))
                .ReturnsAsync(images);

            var result = await service.GetImagesForObject(objId, 2);

            Assert.AreSame(images, result);
        }

        [TestMethod]
        public async Task GetPrimaryImageForObject_ReturnsLowestSortOrder()
        {
            var objId = Guid.NewGuid();

            var images = new ImageCollection
            {
                new Image { SortOrder = 10 },
                new Image { SortOrder = 1 },
                new Image { SortOrder = 5 }
            };

            repoMock
                .Setup(r => r.GetImagesForObject(objId, 2, true))
                .ReturnsAsync(images);

            var result = await service.GetPrimaryImageForObject(objId, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SortOrder);
        }

        [TestMethod]
        public async Task GetPrimaryImageForObject_ReturnsNullWhenEmpty()
        {
            var objId = Guid.NewGuid();
            var images = new ImageCollection();

            repoMock
                .Setup(r => r.GetImagesForObject(objId, 2, true))
                .ReturnsAsync(images);

            var result = await service.GetPrimaryImageForObject(objId, 2);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AddImage_DelegatesToRepository()
        {
            var img = new Image();

            repoMock
                .Setup(r => r.CreateImage(img, true))
                .ReturnsAsync(string.Empty);

            var result = await service.AddImage(img);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public async Task UpdateImage_DelegatesToRepository()
        {
            var img = new Image();

            repoMock
                .Setup(r => r.UpdateImage(img, true))
                .ReturnsAsync(string.Empty);

            var result = await service.UpdateImage(img);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public async Task DeleteImage_DelegatesToRepository()
        {
            var img = new Image();

            repoMock
                .Setup(r => r.DeleteImage(img, true))
                .ReturnsAsync(string.Empty);

            var result = await service.DeleteImage(img);

            Assert.AreEqual(string.Empty, result);
        }
    }
}
