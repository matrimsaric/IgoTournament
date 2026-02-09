using CompetitionDomain.ControlModule.Comparers;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class TournamentCollection : ModelEntryCollection<Tournament>
    {
        public TournamentCollection() : base(true, new TournamentComparer()) { }
        public override Tournament CreateItem() => new();
    }
}
