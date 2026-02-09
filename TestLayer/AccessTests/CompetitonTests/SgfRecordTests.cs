using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class SgfRecordTests
    {
        [TestMethod]
        public void CreateSgfRecord_HasExpectedDefaultValues()
        {
            var sgf = new SgfRecord();

            Assert.IsNotNull(sgf);
            Assert.AreEqual(string.Empty, sgf.Name);
            Assert.AreEqual(Guid.Empty, sgf.MatchId);
            Assert.AreEqual(string.Empty, sgf.SourceUrl);
            Assert.AreEqual(string.Empty, sgf.RawSgf);
            Assert.AreEqual(string.Empty, sgf.ParsedMovesJson);
            Assert.IsNull(sgf.PublishedAt);
            Assert.IsNull(sgf.RetrievedAt);
        }

        [TestMethod]
        public void Properties_CanBeSetAndRetrieved()
        {
            var sgf = new SgfRecord();

            var id = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var published = new DateTime(2024, 6, 1, 10, 0, 0);
            var retrieved = new DateTime(2024, 6, 2, 12, 30, 0);

            sgf.Id = id;
            sgf.Name = "Game 1";
            sgf.MatchId = matchId;
            sgf.SourceUrl = "http://example.com/sgf";
            sgf.RawSgf = "(;GM[1]FF[4])";
            sgf.ParsedMovesJson = "{\"moves\":[]}";
            sgf.PublishedAt = published;
            sgf.RetrievedAt = retrieved;

            Assert.AreEqual(id, sgf.Id);
            Assert.AreEqual("Game 1", sgf.Name);
            Assert.AreEqual(matchId, sgf.MatchId);
            Assert.AreEqual("http://example.com/sgf", sgf.SourceUrl);
            Assert.AreEqual("(;GM[1]FF[4])", sgf.RawSgf);
            Assert.AreEqual("{\"moves\":[]}", sgf.ParsedMovesJson);
            Assert.AreEqual(published, sgf.PublishedAt);
            Assert.AreEqual(retrieved, sgf.RetrievedAt);
        }

        [TestMethod]
        public void Clone_ReturnsDeepCopyWithSameValues()
        {
            var original = new SgfRecord
            {
                Id = Guid.NewGuid(),
                Name = "Game 2",
                MatchId = Guid.NewGuid(),
                SourceUrl = "http://example.com/sgf2",
                RawSgf = "(;B[pd])",
                ParsedMovesJson = "{\"moves\":[\"B[pd]\"]}",
                PublishedAt = new DateTime(2024, 7, 1),
                RetrievedAt = new DateTime(2024, 7, 2)
            };

            var clone = (SgfRecord)original.Clone();

            Assert.IsNotNull(clone);
            Assert.AreNotSame(original, clone);

            Assert.AreEqual(original.Id, clone.Id);
            Assert.AreEqual(original.Name, clone.Name);
            Assert.AreEqual(original.MatchId, clone.MatchId);
            Assert.AreEqual(original.SourceUrl, clone.SourceUrl);
            Assert.AreEqual(original.RawSgf, clone.RawSgf);
            Assert.AreEqual(original.ParsedMovesJson, clone.ParsedMovesJson);
            Assert.AreEqual(original.PublishedAt, clone.PublishedAt);
            Assert.AreEqual(original.RetrievedAt, clone.RetrievedAt);
        }

        // ---------------------------------------------------------
        // GenerateAutoName Tests
        // ---------------------------------------------------------

        [TestMethod]
        public void GenerateAutoName_ProducesExpectedFormat()
        {
            var date = new DateTime(2024, 5, 10);

            var result = SgfRecord.GenerateAutoName(
                date,
                roundNumber: 3,
                boardNumber: 2,
                blackName: "Lee Sedol",
                whiteName: "AlphaGo"
            );

            Assert.AreEqual("2024-05-10_R3_B2_LeeSedol_vs_AlphaGo", result);
        }

        [TestMethod]
        public void GenerateAutoName_HandlesNullDate()
        {
            var result = SgfRecord.GenerateAutoName(
                null,
                roundNumber: 1,
                boardNumber: 1,
                blackName: "Black",
                whiteName: "White"
            );

            Assert.AreEqual("unknown-date_R1_B1_Black_vs_White", result);
        }

        [TestMethod]
        public void GenerateAutoName_HandlesMissingBoardNumber()
        {
            var date = new DateTime(2024, 1, 1);

            var result = SgfRecord.GenerateAutoName(
                date,
                roundNumber: 1,
                boardNumber: null,
                blackName: "Black",
                whiteName: "White"
            );

            Assert.AreEqual("2024-01-01_R1_B?_Black_vs_White", result);
        }

        [TestMethod]
        public void GenerateAutoName_SanitizesInvalidCharacters()
        {
            var result = SgfRecord.GenerateAutoName(
                new DateTime(2024, 8, 20),
                roundNumber: 5,
                boardNumber: 3,
                blackName: "Aki|yama",
                whiteName: "Cho*U"
            );

            // invalid filename chars removed, spaces removed
            Assert.AreEqual("2024-08-20_R5_B3_Akiyama_vs_ChoU", result);
        }

        [TestMethod]
        public void GenerateAutoName_ReplacesEmptyNamesWithUnknown()
        {
            var result = SgfRecord.GenerateAutoName(
                new DateTime(2024, 9, 1),
                roundNumber: 2,
                boardNumber: 4,
                blackName: "",
                whiteName: " "
            );

            Assert.AreEqual("2024-09-01_R2_B4_Unknown_vs_Unknown", result);
        }
    }
}
