using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayerDomain.Model;
using System;

namespace TestLayer.PlayerTests
{
    [TestClass]
    public class PlayerClassTests
    {
        [TestMethod]
        public void Clone_CreatesDeepCopy()
        {
            var original = new Player
            {
                Id = Guid.NewGuid(),
                Name = "Lee Sedol",
                NameJp = "이세돌",
                Rank = "9",
                BirthYear = 1983,
                Affiliation = "KBA",
                Notes = "Legendary player"
            };

            var clone = (Player)original.Clone();

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.NameJp, clone.NameJp);
            Assert.AreEqual(original.Rank, clone.Rank);
            Assert.AreEqual(original.BirthYear, clone.BirthYear);
            Assert.AreEqual(original.Affiliation, clone.Affiliation);
            Assert.AreEqual(original.Notes, clone.Notes);

            // Ensure it's a different object
            Assert.AreNotSame(original, clone);
        }

        [TestMethod]
        public void CompareTo_SortsAlphabeticallyByName()
        {
            var a = new Player { Name = "Alpha" };
            var b = new Player { Name = "Beta" };

            var result = a.CompareTo(b);

            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_ReturnsPositive_WhenOtherIsNull()
        {
            var player = new Player { Name = "Test" };

            var result = player.CompareTo(null);

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            var p = new Player();

            Assert.AreEqual(string.Empty, p.Name);
            Assert.AreEqual(string.Empty, p.NameJp);
            Assert.AreEqual(string.Empty, p.Rank);
            Assert.AreEqual(string.Empty, p.Affiliation);
            Assert.AreEqual(string.Empty, p.Notes);
            Assert.IsNull(p.BirthYear);
        }
    }
}
