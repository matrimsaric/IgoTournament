using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageDomain.Model;
using System;
using System.Linq;

namespace TestLayer.ImageTests
{
    [TestClass]
    public class ImageCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewImage()
        {
            var collection = new ImageCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Image));
        }

        [TestMethod]
        public void Collection_DoesNotSortByDefault()
        {
            var c = new ImageCollection();

            var img1 = new Image { Id = Guid.NewGuid(), SortOrder = 10 };
            var img2 = new Image { Id = Guid.NewGuid(), SortOrder = 1 };

            c.Add(img1);
            c.Add(img2);

            var list = c.ToList();

            Assert.AreEqual(img1.Id, list[0].Id);
            Assert.AreEqual(img2.Id, list[1].Id);
        }

        [TestMethod]
        public void CompareTo_UsesSortOrder()
        {
            var a = new Image { SortOrder = 1 };
            var b = new Image { SortOrder = 5 };

            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        [TestMethod]
        public void Clone_CopiesAllFields()
        {
            var img = new Image
            {
                Id = Guid.NewGuid(),
                ObjectId = Guid.NewGuid(),
                ObjectType = 2,
                ImageUrl = "abc",
                SizeType = 3,
                SortOrder = 10,
                Notes = "test"
            };

            var clone = (Image)img.Clone();

            Assert.AreEqual(img.Id, clone.Id);
            Assert.AreEqual(img.ObjectId, clone.ObjectId);
            Assert.AreEqual(img.ObjectType, clone.ObjectType);
            Assert.AreEqual(img.ImageUrl, clone.ImageUrl);
            Assert.AreEqual(img.SizeType, clone.SizeType);
            Assert.AreEqual(img.SortOrder, clone.SortOrder);
            Assert.AreEqual(img.Notes, clone.Notes);
        }
    }
}
