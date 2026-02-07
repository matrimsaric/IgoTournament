#pragma warning disable
using System;
using System.Collections.Generic;
using System.Text;
using Match = CompetitionDomain.Model.Match;
using String = System.String;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class MatchComparer : IComparer<Match>
    {
        public int Compare(Match? x, Match? y)
        {
            if (x == null || y == null)
                return 0;

            int r = x.RoundId.CompareTo(y.RoundId);
            if (r != 0)
                return r;

            int b = x.BoardNumber.CompareTo(y.BoardNumber);
            if (b != 0)
                return b;

            // Final fallback to ensure stable ordering
            return x.Id.CompareTo(y.Id);
        }
    }
}
