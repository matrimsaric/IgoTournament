using JosekiDomain.Model;
using JosekiDomain.ControlModule.Comparers;
using ServerCommonModule.Model;

namespace JosekiDomain.Model
{
    public class JosekiEntryCollection : ModelEntryCollection<JosekiEntry>
    {
        public JosekiEntryCollection() : base(true, new JosekiEntryComparer()) { }

        public override JosekiEntry CreateItem() => new();
    }
}
