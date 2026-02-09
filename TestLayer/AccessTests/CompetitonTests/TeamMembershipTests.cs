using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TeamMembershipTests
    {
        [TestMethod]
        public void CreateTeamMembership_HasExpectedDefaultValues()
        {
            var tm = new TeamMembership();

            Assert.IsNotNull(tm);
            Assert.AreEqual(string.Empty, tm.Name);
            Assert.AreEqual(Guid.Empty, tm.PlayerId);
            Assert.AreEqual(Guid.Empty, tm.TeamId);
            Assert.AreEqual(string.Empty, tm.Season);
            Assert.AreEqual(string.Empty, tm.Role);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var tm = new TeamMembership();

            var id = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            tm.Id = id;
            tm.Name = "Team Member";
            tm.PlayerId = playerId;
            tm.TeamId = teamId;
            tm.Season = "2024";
            tm.Role = "Captain";

            Assert.AreEqual(id, tm.Id);
            Assert.AreEqual("Team Member", tm.Name);
            Assert.AreEqual(playerId, tm.PlayerId);
            Assert.AreEqual(teamId, tm.TeamId);
            Assert.AreEqual("2024", tm.Season);
            Assert.AreEqual("Captain", tm.Role);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new TeamMembership
            {
                Id = Guid.NewGuid(),
                Name = "Original",
                PlayerId = Guid.NewGuid(),
                TeamId = Guid.NewGuid(),
                Season = "2023",
                Role = "Vice-Captain"
            };

            var clone = (TeamMembership)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.PlayerId, clone.PlayerId);
            Assert.AreEqual(original.TeamId, clone.TeamId);
            Assert.AreEqual(original.Season, clone.Season);
            Assert.AreEqual(original.Role, clone.Role);
        }
    }
}
