using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void CreateMatch_HasExpectedDefaultValues()
        {
            var match = new Match();

            Assert.IsNotNull(match);
            Assert.AreEqual(string.Empty, match.Name);
            Assert.AreEqual(0, match.BoardNumber);
            Assert.AreEqual(string.Empty, match.Result);
            Assert.AreEqual(string.Empty, match.Notes);

            Assert.AreEqual(Guid.Empty, match.RoundId);
            Assert.AreEqual(Guid.Empty, match.BlackPlayerId);
            Assert.AreEqual(Guid.Empty, match.WhitePlayerId);

            Assert.IsNull(match.WinnerId);
            Assert.IsNull(match.SgfId);
            Assert.IsNull(match.GameDate);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new Match
            {
                Id = Guid.NewGuid(),
                Name = "Board 3",
                RoundId = Guid.NewGuid(),
                BoardNumber = 3,
                BlackPlayerId = Guid.NewGuid(),
                WhitePlayerId = Guid.NewGuid(),
                Result = "B+R",
                WinnerId = Guid.NewGuid(),
                SgfId = Guid.NewGuid(),
                GameDate = new DateTime(2024, 5, 12),
                Notes = "Exciting mid‑game fight"
            };

            var clone = (Match)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.RoundId, clone.RoundId);
            Assert.AreEqual(original.BoardNumber, clone.BoardNumber);
            Assert.AreEqual(original.BlackPlayerId, clone.BlackPlayerId);
            Assert.AreEqual(original.WhitePlayerId, clone.WhitePlayerId);
            Assert.AreEqual(original.Result, clone.Result);
            Assert.AreEqual(original.WinnerId, clone.WinnerId);
            Assert.AreEqual(original.SgfId, clone.SgfId);
            Assert.AreEqual(original.GameDate, clone.GameDate);
            Assert.AreEqual(original.Notes, clone.Notes);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var match = new Match();

            var id = Guid.NewGuid();
            var roundId = Guid.NewGuid();
            var blackId = Guid.NewGuid();
            var whiteId = Guid.NewGuid();
            var winnerId = Guid.NewGuid();
            var sgfId = Guid.NewGuid();
            var date = new DateTime(2024, 1, 1);

            match.Id = id;
            match.Name = "Test Match";
            match.RoundId = roundId;
            match.BoardNumber = 5;
            match.BlackPlayerId = blackId;
            match.WhitePlayerId = whiteId;
            match.Result = "W+3.5";
            match.WinnerId = winnerId;
            match.SgfId = sgfId;
            match.GameDate = date;
            match.Notes = "Some notes";

            Assert.AreEqual(id, match.Id);
            Assert.AreEqual("Test Match", match.Name);
            Assert.AreEqual(roundId, match.RoundId);
            Assert.AreEqual(5, match.BoardNumber);
            Assert.AreEqual(blackId, match.BlackPlayerId);
            Assert.AreEqual(whiteId, match.WhitePlayerId);
            Assert.AreEqual("W+3.5", match.Result);
            Assert.AreEqual(winnerId, match.WinnerId);
            Assert.AreEqual(sgfId, match.SgfId);
            Assert.AreEqual(date, match.GameDate);
            Assert.AreEqual("Some notes", match.Notes);
        }
    }
}
