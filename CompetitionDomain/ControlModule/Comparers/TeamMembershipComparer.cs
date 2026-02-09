using CompetitionDomain.Model;
using System;
using System.Collections.Generic;

namespace CompetitionDomain.ControlModule.Comparers
{
    public class TeamMembershipComparer : IComparer<TeamMembership>
    {
        public int Compare(TeamMembership? x, TeamMembership? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int s = string.Compare(x.Season, y.Season, StringComparison.Ordinal);
            if (s != 0) return s;

            int t = x.TeamId.CompareTo(y.TeamId);
            if (t != 0) return t;

            return x.PlayerId.CompareTo(y.PlayerId);
        }
    }
}
