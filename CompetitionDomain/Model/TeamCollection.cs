using ServerCommonModule.Model;

namespace CompetitionDomain.Model
{
    public class TeamCollection : ModelEntryCollection<Team>
    {
        public TeamCollection() : base(true, null) { }

        public override Team CreateItem() => new();
    }
}
