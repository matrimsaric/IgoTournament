using CompetitionDomain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class TeamComparer : IComparer<Team>
    {
        public int Compare(Team? x, Team? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int n = string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            if (n != 0) return n;

            // Secondary key ensures duplicates are NOT treated as equal
            return x.Id.CompareTo(y.Id);
        }
    }

}
