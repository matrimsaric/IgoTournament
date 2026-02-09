using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TeamMembershipCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewTeamMembership()
        {
            var collection = new TeamMembershipCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(TeamMembership));
        }

        [TestMethod]
        public void Collection_SortsBySeason_ThenTeam_ThenPlayer()
        {
            var collection = new TeamMembershipCollection();

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();
            var p1 = Guid.NewGuid();
            var p2 = Guid.NewGuid();

            var m1 = new TeamMembership { Id = Guid.NewGuid(), Season = "2024", TeamId = t2, PlayerId = p1 };
            var m2 = new TeamMembership { Id = Guid.NewGuid(), Season = "2023", TeamId = t1, PlayerId = p2 };
            var m3 = new TeamMembership { Id = Guid.NewGuid(), Season = "2023", TeamId = t1, PlayerId = p1 };

            collection.Add(m1);
            collection.Add(m2);
            collection.Add(m3);

            var ordered = collection.ToList();

            var expected = new[] { m1, m2, m3 }
                .OrderBy(m => m.Season)
                .ThenBy(m => m.TeamId)
                .ThenBy(m => m.PlayerId)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }

        [TestMethod]
        public void AddingMemberships_AllowsDuplicatesAndSortsCorrectly()
        {
            var collection = new TeamMembershipCollection();

            var season = "2024";
            var team = Guid.NewGuid();

            var m1 = new TeamMembership { Id = Guid.NewGuid(), Season = season, TeamId = team, PlayerId = Guid.NewGuid() };
            var m2 = new TeamMembership { Id = Guid.NewGuid(), Season = season, TeamId = team, PlayerId = Guid.NewGuid() };
            var m3 = new TeamMembership { Id = Guid.NewGuid(), Season = season, TeamId = team, PlayerId = Guid.NewGuid() };

            collection.Add(m1);
            collection.Add(m2);
            collection.Add(m3);

            var ordered = collection.ToList();

            var expected = new[] { m1, m2, m3 }
                .OrderBy(m => m.PlayerId)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }
    }
}
