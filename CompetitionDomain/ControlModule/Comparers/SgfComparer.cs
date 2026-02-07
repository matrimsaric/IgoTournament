using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class SgfRecordComparer : IComparer<SgfRecord>
    {
        public int Compare(SgfRecord? x, SgfRecord? y)
        {
            if (x == null || y == null) return 0;

            int m = x.MatchId.CompareTo(y.MatchId);
            if (m != 0) return m;

            int p = Nullable.Compare(x.PublishedAt, y.PublishedAt);
            if (p != 0) return p;

            return Nullable.Compare(x.RetrievedAt, y.RetrievedAt);
        }
    }

}
