using CompetitionDomain.ControlModule.Comparers;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class RoundCollection : ModelEntryCollection<Round>
    {
        public RoundCollection() : base(true, new RoundComparer()) { }
        public override Round CreateItem() => new();
    }
}
