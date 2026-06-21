using JosekiDomain.Model;
using JosekiDomain.ControlModule.Comparers;
using ServerCommonModule.Model;

namespace JosekiDomain.Model
{
    public class JosekiBookCollection : ModelEntryCollection<JosekiBook>
    {
        public JosekiBookCollection() : base(true, new JosekiBookComparer()) { }

        public override JosekiBook CreateItem() => new();
    }
}
