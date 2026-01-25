using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerDomain.Model
{
    public class TeamCollection : ModelEntryCollection<Team>
    {
        public TeamCollection() : base(true, null) { }
        public override Team CreateItem() => new();
    }
}
