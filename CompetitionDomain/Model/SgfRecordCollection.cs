using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class SgfRecordCollection : ModelEntryCollection<SgfRecord>
    {
        public SgfRecordCollection() : base(false, null) { }
        public override SgfRecord CreateItem() => new();
    }
}
