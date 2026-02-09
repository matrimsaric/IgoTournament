using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TeamTests
    {
        [TestMethod]
        public void CreateTeam_HasExpectedDefaultValues()
        {
            var team = new Team();

            Assert.IsNotNull(team);
            Assert.AreEqual(string.Empty, team.Name);
            Assert.AreEqual(string.Empty, team.NameJp);
            Assert.AreEqual(string.Empty, team.ColourPrimary);
            Assert.AreEqual(string.Empty, team.ColourSecondary);
            Assert.AreEqual(string.Empty, team.Notes);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var team = new Team();

            var id = Guid.NewGuid();

            team.Id = id;
            team.Name = "Kansai Ki-in";
            team.NameJp = "関西棋院";
            team.ColourPrimary = "Blue";
            team.ColourSecondary = "White";
            team.Notes = "Strong regional team";

            Assert.AreEqual(id, team.Id);
            Assert.AreEqual("Kansai Ki-in", team.Name);
            Assert.AreEqual("関西棋院", team.NameJp);
            Assert.AreEqual("Blue", team.ColourPrimary);
            Assert.AreEqual("White", team.ColourSecondary);
            Assert.AreEqual("Strong regional team", team.Notes);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Tokyo",
                NameJp = "東京",
                ColourPrimary = "Red",
                ColourSecondary = "Black",
                Notes = "Defending champions"
            };

            var clone = (Team)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.NameJp, clone.NameJp);
            Assert.AreEqual(original.ColourPrimary, clone.ColourPrimary);
            Assert.AreEqual(original.ColourSecondary, clone.ColourSecondary);
            Assert.AreEqual(original.Notes, clone.Notes);
        }

        [TestMethod]
        public void CompareTo_OrdersByName()
        {
            var a = new Team { Name = "Alpha" };
            var b = new Team { Name = "Bravo" };

            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
            Assert.AreEqual(0, a.CompareTo(new Team { Name = "Alpha" }));
        }

        [TestMethod]
        public void CompareTo_ReturnsPositive_WhenOtherIsNull()
        {
            var team = new Team { Name = "Any" };

            Assert.IsTrue(team.CompareTo(null) > 0);
        }
    }
}
