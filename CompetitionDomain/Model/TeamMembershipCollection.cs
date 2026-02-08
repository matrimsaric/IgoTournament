using CompetitionDomain.Model;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class TeamMembershipCollection : ModelEntryCollection<TeamMembership>
    {
        public TeamMembershipCollection() : base(false, null) { }
        public override TeamMembership CreateItem() => new();
    }
}
