using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class RoundTests
    {
        [TestMethod]
        public void CreateRound_HasExpectedDefaultValues()
        {
            var round = new Round();

            Assert.IsNotNull(round);
            Assert.AreEqual(string.Empty, round.Name);
            Assert.AreEqual(Guid.Empty, round.TournamentId);
            Assert.AreEqual(0, round.RoundNumber);
            Assert.IsNull(round.RoundDate);
            Assert.AreEqual(string.Empty, round.Notes);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var round = new Round();

            var id = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();
            var date = new DateTime(2024, 3, 15);

            round.Id = id;
            round.Name = "Round 4";
            round.TournamentId = tournamentId;
            round.RoundNumber = 4;
            round.RoundDate = date;
            round.Notes = "Important playoff round";

            Assert.AreEqual(id, round.Id);
            Assert.AreEqual("Round 4", round.Name);
            Assert.AreEqual(tournamentId, round.TournamentId);
            Assert.AreEqual(4, round.RoundNumber);
            Assert.AreEqual(date, round.RoundDate);
            Assert.AreEqual("Important playoff round", round.Notes);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new Round
            {
                Id = Guid.NewGuid(),
                Name = "Round 2",
                TournamentId = Guid.NewGuid(),
                RoundNumber = 2,
                RoundDate = new DateTime(2024, 2, 10),
                Notes = "Opening round"
            };

            var clone = (Round)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.TournamentId, clone.TournamentId);
            Assert.AreEqual(original.RoundNumber, clone.RoundNumber);
            Assert.AreEqual(original.RoundDate, clone.RoundDate);
            Assert.AreEqual(original.Notes, clone.Notes);
        }
    }
}
