using CompetitionDomain.ControlModule.Comparers;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class MatchCollection : ModelEntryCollection<Match>
    {
        public MatchCollection() : base(true, new MatchComparer()) { }
        public override Match CreateItem() => new();
    }

}
