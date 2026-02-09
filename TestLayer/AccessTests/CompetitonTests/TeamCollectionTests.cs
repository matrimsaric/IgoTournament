using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TeamCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewTeam()
        {
            var collection = new TeamCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(Team));
        }

        [TestMethod]
        public void Collection_SortsUsingTeamComparer()
        {
            var collection = new TeamCollection();

            var t1 = new Team { Name = "Charlie" };
            var t2 = new Team { Name = "Alpha" };
            var t3 = new Team { Name = "Bravo" };

            collection.Add(t1);
            collection.Add(t2);
            collection.Add(t3);

            var ordered = collection.ToList();

            Assert.AreEqual("Alpha", ordered[0].Name);
            Assert.AreEqual("Bravo", ordered[1].Name);
            Assert.AreEqual("Charlie", ordered[2].Name);
        }

        [TestMethod]
        public void AddingTeams_MaintainsSortedOrder()
        {
            var collection = new TeamCollection();

            collection.Add(new Team { Name = "Delta" });
            collection.Add(new Team { Name = "Alpha" });
            collection.Add(new Team { Name = "Charlie" });

            var ordered = collection.ToList();

            Assert.AreEqual("Alpha", ordered[0].Name);
            Assert.AreEqual("Charlie", ordered[1].Name);
            Assert.AreEqual("Delta", ordered[2].Name);
        }

        [TestMethod]
        public void AddingTeams_AllowsDuplicateNames()
        {
            var collection = new TeamCollection();

            var t1 = new Team { Id = Guid.NewGuid(), Name = "Alpha" };
            var t2 = new Team { Id = Guid.NewGuid(), Name = "Alpha" };
            var t3 = new Team { Id = Guid.NewGuid(), Name = "Bravo" };

            collection.Add(t1);
            collection.Add(t2);
            collection.Add(t3);

            var ordered = collection.ToList();

            Assert.AreEqual(3, ordered.Count);

            Assert.AreEqual(2, ordered.Count(t => t.Name == "Alpha"));
            Assert.AreEqual(1, ordered.Count(t => t.Name == "Bravo"));

            // Sorted by Name
            for (int i = 0; i < ordered.Count - 1; i++)
            {
                Assert.IsTrue(
                    string.Compare(ordered[i].Name, ordered[i + 1].Name, StringComparison.Ordinal) <= 0
                );
            }
        }

    }
}
