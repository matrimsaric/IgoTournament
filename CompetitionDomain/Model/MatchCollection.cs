using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class MatchCollection : ModelEntryCollection<Match>
    {
        public MatchCollection() : base(true, null) { }
        public override Match CreateItem() => new();
    }

}
