using ServerCommonModule.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.Model
{
    public class ArchiveStatusCollection : ModelEntryCollection<ArchiveStatus>
    {
        // TODO Need comparers for the other classes like the PlayerComparer
        public ArchiveStatusCollection() : base(false, null) { }
        public override ArchiveStatus CreateItem() => new();
    }
}

