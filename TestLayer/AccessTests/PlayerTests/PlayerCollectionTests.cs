using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayerDomain.Model;
using System;
using System.Linq;

namespace TestLayer.PlayerTests
{
    [TestClass]
    public class PlayerCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewPlayer()
        {
            var collection = new PlayerCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Player));
        }

        [TestMethod]
        public void Collection_SortsUsingPlayerComparer()
        {
            var collection = new PlayerCollection();

            var p1 = new Player { Name = "Charlie" };
            var p2 = new Player { Name = "Alpha" };
            var p3 = new Player { Name = "Bravo" };

            collection.Add(p1);
            collection.Add(p2);
            collection.Add(p3);

            var ordered = collection.ToList();

            Assert.AreEqual("Alpha", ordered[0].Name);
            Assert.AreEqual("Bravo", ordered[1].Name);
            Assert.AreEqual("Charlie", ordered[2].Name);
        }

        [TestMethod]
        public void AddingPlayers_MaintainsSortedOrder()
        {
            var collection = new PlayerCollection();

            collection.Add(new Player { Name = "Delta" });
            collection.Add(new Player { Name = "Alpha" });
            collection.Add(new Player { Name = "Charlie" });

            var ordered = collection.ToList();

            Assert.AreEqual("Alpha", ordered[0].Name);
            Assert.AreEqual("Charlie", ordered[1].Name);
            Assert.AreEqual("Delta", ordered[2].Name);
        }
    }
}
