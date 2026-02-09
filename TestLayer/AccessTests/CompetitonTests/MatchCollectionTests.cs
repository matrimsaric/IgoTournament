using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class MatchCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewMatch()
        {
            var collection = new MatchCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Match));
        }

        [TestMethod]
        public void Collection_SortsByRoundId_ThenBoardNumber()
        {
            var collection = new MatchCollection();

            var r1 = Guid.NewGuid();
            var r2 = Guid.NewGuid();

            var m1 = new Match { Name = "A", RoundId = r2, BoardNumber = 3 };
            var m2 = new Match { Name = "B", RoundId = r1, BoardNumber = 2 };
            var m3 = new Match { Name = "C", RoundId = r1, BoardNumber = 1 };

            collection.Add(m1);
            collection.Add(m2);
            collection.Add(m3);

            var ordered = collection.ToList();

            // RoundId ascending
            var expected = new[] { m1, m2, m3 }
    .OrderBy(m => m.RoundId)
    .ThenBy(m => m.BoardNumber)
    .ThenBy(m => m.Id)
    .ToList();

            Assert.AreEqual(expected[0].RoundId, ordered[0].RoundId);
            Assert.AreEqual(expected[1].RoundId, ordered[1].RoundId);
            Assert.AreEqual(expected[2].RoundId, ordered[2].RoundId);

            Assert.AreEqual(expected[0].BoardNumber, ordered[0].BoardNumber);
            Assert.AreEqual(expected[1].BoardNumber, ordered[1].BoardNumber);
            Assert.AreEqual(expected[2].BoardNumber, ordered[2].BoardNumber);
        }

        [TestMethod]
        public void AddingMatches_MaintainsComparerOrder()
        {
            var collection = new MatchCollection();

            var round = Guid.NewGuid();

            collection.Add(new Match { RoundId = round, BoardNumber = 5 });
            collection.Add(new Match { RoundId = round, BoardNumber = 1 });
            collection.Add(new Match { RoundId = round, BoardNumber = 3 });

            var ordered = collection.ToList();

            Assert.AreEqual(1, ordered[0].BoardNumber);
            Assert.AreEqual(3, ordered[1].BoardNumber);
            Assert.AreEqual(5, ordered[2].BoardNumber);
        }

        [TestMethod]
        public void AddingMatches_AllowsDuplicatesAndSortsByIdAsFallback()
        {
            var collection = new MatchCollection();

            var round = Guid.NewGuid();

            var m1 = new Match { RoundId = round, BoardNumber = 1, Id = Guid.NewGuid() };
            var m2 = new Match { RoundId = round, BoardNumber = 1, Id = Guid.NewGuid() };
            var m3 = new Match { RoundId = round, BoardNumber = 1, Id = Guid.NewGuid() };

            collection.Add(m1);
            collection.Add(m2);
            collection.Add(m3);

            var ordered = collection.ToList();

            // All equal on RoundId and BoardNumber → sorted by Id
            var sortedIds = new[] { m1.Id, m2.Id, m3.Id }.OrderBy(id => id).ToList();

            Assert.AreEqual(sortedIds[0], ordered[0].Id);
            Assert.AreEqual(sortedIds[1], ordered[1].Id);
            Assert.AreEqual(sortedIds[2], ordered[2].Id);
        }
    }
}
