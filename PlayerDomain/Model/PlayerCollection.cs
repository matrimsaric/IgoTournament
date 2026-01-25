using PlayerDomain.ControlModule.Comparers;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TableAttribute = ServerCommonModule.Attributes.TableAttribute;

namespace PlayerDomain.Model
{
    public class PlayerCollection : ModelEntryCollection<Player>
    {
        public PlayerCollection() : base(true, new PlayerComparer()) { }
        public override Player CreateItem() => new();
    }
}
