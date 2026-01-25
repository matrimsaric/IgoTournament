using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class RoundCollection : ModelEntryCollection<Round>
    {
        public RoundCollection() : base(true, null) { }
        public override Round CreateItem() => new();
    }
}
