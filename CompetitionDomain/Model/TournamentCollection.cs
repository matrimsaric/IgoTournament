using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class TournamentCollection : ModelEntryCollection<Tournament>
    {
        public TournamentCollection() : base(true, null) { }
        public override Tournament CreateItem() => new();
    }
}
