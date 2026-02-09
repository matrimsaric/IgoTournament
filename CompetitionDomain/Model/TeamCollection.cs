using CompetitionDomain.ControlModule.Comparers;
using ServerCommonModule.Model;

namespace CompetitionDomain.Model
{
    public class TeamCollection : ModelEntryCollection<Team>
    {
        public TeamCollection() : base(true, new TeamComparer()) { }

        public override Team CreateItem() => new();
    }
}
