using CompetitionDomain.ControlModule.Comparers;
using CompetitionDomain.Model;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class TeamMembershipCollection : ModelEntryCollection<TeamMembership>
    {
        public TeamMembershipCollection() : base(true, new TeamMembershipComparer()) { }
        public override TeamMembership CreateItem() => new();
    }
}
