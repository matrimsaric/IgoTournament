using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class RoundCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewRound()
        {
            var collection = new RoundCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Round));
        }

        [TestMethod]
        public void Collection_SortsByTournamentId_ThenRoundNumber_ThenRoundDate()
        {
            var collection = new RoundCollection();

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();

            var r1 = new Round { TournamentId = t2, RoundNumber = 3, RoundDate = new DateTime(2024, 5, 10) };
            var r2 = new Round { TournamentId = t1, RoundNumber = 2, RoundDate = new DateTime(2024, 5, 11) };
            var r3 = new Round { TournamentId = t1, RoundNumber = 2, RoundDate = new DateTime(2024, 5, 9) };

            collection.Add(r1);
            collection.Add(r2);
            collection.Add(r3);

            var ordered = collection.ToList();

            var expected = new[] { r1, r2, r3 }
                .OrderBy(r => r.TournamentId)
                .ThenBy(r => r.RoundNumber)
                .ThenBy(r => r.RoundDate)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }

        [TestMethod]
        public void AddingRounds_MaintainsComparerOrder()
        {
            var collection = new RoundCollection();

            var t = Guid.NewGuid();

            collection.Add(new Round { TournamentId = t, RoundNumber = 5 });
            collection.Add(new Round { TournamentId = t, RoundNumber = 1 });
            collection.Add(new Round { TournamentId = t, RoundNumber = 3 });

            var ordered = collection.ToList();

            var expected = ordered
                .OrderBy(r => r.RoundNumber)
                .ToList();

            CollectionAssert.AreEqual(expected.Select(r => r.Id).ToList(),
                                      ordered.Select(r => r.Id).ToList());
        }

        [TestMethod]
        public void AddingRounds_AllowsDuplicatesAndSortsByRoundDate()
        {
            var collection = new RoundCollection();

            var t = Guid.NewGuid();

            var r1 = new Round { TournamentId = t, RoundNumber = 1, RoundDate = new DateTime(2024, 1, 10) };
            var r2 = new Round { TournamentId = t, RoundNumber = 1, RoundDate = new DateTime(2024, 1, 5) };
            var r3 = new Round { TournamentId = t, RoundNumber = 1, RoundDate = new DateTime(2024, 1, 20) };

            collection.Add(r1);
            collection.Add(r2);
            collection.Add(r3);

            var ordered = collection.ToList();

            var expected = new[] { r1, r2, r3 }
                .OrderBy(r => r.RoundDate)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }
    }
}
