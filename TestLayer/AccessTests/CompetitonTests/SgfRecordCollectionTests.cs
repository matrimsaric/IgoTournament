using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompetitionDomain.Model;
using System;
using System.Linq;

namespace TestLayer.CompetitionTests
{
    [TestClass]
    public class SgfRecordCollectionTests
    {
        [TestMethod]
        public void CreateItem_ReturnsNewSgfRecord()
        {
            var collection = new SgfRecordCollection();

            var item = collection.CreateItem();

            Assert.IsNotNull(item);
            Assert.IsInstanceOfType(item, typeof(SgfRecord));
        }

        [TestMethod]
        public void Collection_SortsByMatchId_ThenPublishedAt_ThenRetrievedAt()
        {
            var collection = new SgfRecordCollection();

            var m1 = Guid.NewGuid();
            var m2 = Guid.NewGuid();

            var r1 = new SgfRecord
            {
                MatchId = m2,
                PublishedAt = new DateTime(2024, 5, 10),
                RetrievedAt = new DateTime(2024, 5, 11)
            };

            var r2 = new SgfRecord
            {
                MatchId = m1,
                PublishedAt = new DateTime(2024, 5, 12),
                RetrievedAt = new DateTime(2024, 5, 13)
            };

            var r3 = new SgfRecord
            {
                MatchId = m1,
                PublishedAt = new DateTime(2024, 5, 9),
                RetrievedAt = new DateTime(2024, 5, 10)
            };

            collection.Add(r1);
            collection.Add(r2);
            collection.Add(r3);

            var ordered = collection.ToList();

            var expected = new[] { r1, r2, r3 }
                .OrderBy(r => r.MatchId)
                .ThenBy(r => r.PublishedAt)
                .ThenBy(r => r.RetrievedAt)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }

        [TestMethod]
        public void AddingRecords_MaintainsComparerOrder()
        {
            var collection = new SgfRecordCollection();

            var match = Guid.NewGuid();

            collection.Add(new SgfRecord { MatchId = match, PublishedAt = new DateTime(2024, 1, 10) });
            collection.Add(new SgfRecord { MatchId = match, PublishedAt = new DateTime(2024, 1, 1) });
            collection.Add(new SgfRecord { MatchId = match, PublishedAt = new DateTime(2024, 1, 5) });

            var ordered = collection.ToList();

            var expected = ordered
                .OrderBy(r => r.PublishedAt)
                .ToList();

            CollectionAssert.AreEqual(
                expected.Select(r => r.Id).ToList(),
                ordered.Select(r => r.Id).ToList()
            );
        }

        [TestMethod]
        public void AddingRecords_AllowsDuplicatesAndSortsByRetrievedAt()
        {
            var collection = new SgfRecordCollection();

            var match = Guid.NewGuid();
            var pub = new DateTime(2024, 3, 1);

            var r1 = new SgfRecord { MatchId = match, PublishedAt = pub, RetrievedAt = new DateTime(2024, 3, 10) };
            var r2 = new SgfRecord { MatchId = match, PublishedAt = pub, RetrievedAt = new DateTime(2024, 3, 5) };
            var r3 = new SgfRecord { MatchId = match, PublishedAt = pub, RetrievedAt = new DateTime(2024, 3, 20) };

            collection.Add(r1);
            collection.Add(r2);
            collection.Add(r3);

            var ordered = collection.ToList();

            var expected = new[] { r1, r2, r3 }
                .OrderBy(r => r.RetrievedAt)
                .ToList();

            Assert.AreEqual(expected[0].Id, ordered[0].Id);
            Assert.AreEqual(expected[1].Id, ordered[1].Id);
            Assert.AreEqual(expected[2].Id, ordered[2].Id);
        }
    }
}
