using CompetitionDomain.Model;
using System;
using System.Collections.Generic;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class TournamentComparer : IComparer<Tournament>
    {
        public int Compare(Tournament? x, Tournament? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int n = string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            if (n != 0) return n;

            return string.Compare(x.Season, y.Season, StringComparison.Ordinal);
        }
    }
}
