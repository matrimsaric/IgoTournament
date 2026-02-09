using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TournamentCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewTournament()
        {
            var collection = new TournamentCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Tournament));
        }

        [TestMethod]
        public void Collection_SortsByName_ThenSeason()
        {
            var collection = new TournamentCollection();

            var t1 = new Tournament { Id = Guid.NewGuid(), Name = "Bravo Cup", Season = "2024" };
            var t2 = new Tournament { Id = Guid.NewGuid(), Name = "Alpha Open", Season = "2023" };
            var t3 = new Tournament { Id = Guid.NewGuid(), Name = "Alpha Open", Season = "2024" };

            collection.Add(t1);
            collection.Add(t2);
            collection.Add(t3);

            var ordered = collection.ToList();

            var expected = new[] { t1, t2, t3 }
                .OrderBy(t => t.Name)
                .ThenBy(t => t.Season)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }

        [TestMethod]
        public void AddingTournaments_AllowsDuplicatesAndSortsCorrectly()
        {
            var collection = new TournamentCollection();

            var name = "Li League";

            var t1 = new Tournament { Id = Guid.NewGuid(), Name = name, Season = "2023" };
            var t2 = new Tournament { Id = Guid.NewGuid(), Name = name, Season = "2025" };
            var t3 = new Tournament { Id = Guid.NewGuid(), Name = name, Season = "2024" };

            collection.Add(t1);
            collection.Add(t2);
            collection.Add(t3);

            var ordered = collection.ToList();

            var expected = new[] { t1, t2, t3 }
                .OrderBy(t => t.Season)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }
    }
}
