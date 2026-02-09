using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class TournamentTests
    {
        [TestMethod]
        public void CreateTournament_HasExpectedDefaultValues()
        {
            var t = new Tournament();

            Assert.IsNotNull(t);
            Assert.AreEqual(string.Empty, t.Name);
            Assert.AreEqual(string.Empty, t.NameJp);
            Assert.AreEqual(string.Empty, t.Season);
            Assert.AreEqual(string.Empty, t.Organiser);
            Assert.AreEqual(string.Empty, t.Ruleset);
            Assert.IsNull(t.Komi);
            Assert.AreEqual(string.Empty, t.TimeSettings);
            Assert.AreEqual(string.Empty, t.Notes);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var t = new Tournament();

            var id = Guid.NewGuid();

            t.Id = id;
            t.Name = "Li League";
            t.NameJp = "リーグ";
            t.Season = "2024";
            t.Organiser = "Japan Go Association";
            t.Ruleset = "Japanese";
            t.Komi = 6.5m;
            t.TimeSettings = "60m + byo-yomi";
            t.Notes = "Top-tier league";

            Assert.AreEqual(id, t.Id);
            Assert.AreEqual("Li League", t.Name);
            Assert.AreEqual("リーグ", t.NameJp);
            Assert.AreEqual("2024", t.Season);
            Assert.AreEqual("Japan Go Association", t.Organiser);
            Assert.AreEqual("Japanese", t.Ruleset);
            Assert.AreEqual(6.5m, t.Komi);
            Assert.AreEqual("60m + byo-yomi", t.TimeSettings);
            Assert.AreEqual("Top-tier league", t.Notes);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = "Kansai Open",
                NameJp = "関西オープン",
                Season = "2023",
                Organiser = "Kansai Ki-in",
                Ruleset = "Japanese",
                Komi = 6.5m,
                TimeSettings = "45m + 30s x 3",
                Notes = "Regional event"
            };

            var clone = (Tournament)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.NameJp, clone.NameJp);
            Assert.AreEqual(original.Season, clone.Season);
            Assert.AreEqual(original.Organiser, clone.Organiser);
            Assert.AreEqual(original.Ruleset, clone.Ruleset);
            Assert.AreEqual(original.Komi, clone.Komi);
            Assert.AreEqual(original.TimeSettings, clone.TimeSettings);
            Assert.AreEqual(original.Notes, clone.Notes);
        }
    }
}
