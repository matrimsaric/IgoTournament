using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class RoundComparer : IComparer<Round>
    {
        public int Compare(Round? x, Round? y)
        {
            if (x == null || y == null) return 0;

            int t = x.TournamentId.CompareTo(y.TournamentId);
            if (t != 0) return t;

            int r = x.RoundNumber.CompareTo(y.RoundNumber);
            if (r != 0) return r;

            return Nullable.Compare(x.RoundDate, y.RoundDate);
        }
    }

}
