using CompetitionDomain.ControlModule.Comparers;
using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class SgfRecordCollection : ModelEntryCollection<SgfRecord>
    {
        public SgfRecordCollection() : base(false, new SgfRecordComparer()) { }
        public override SgfRecord CreateItem() => new();
    }
}
