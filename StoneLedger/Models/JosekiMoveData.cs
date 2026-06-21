using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Models
{
    public class JosekiMoveData
    {
        public List<SgfMove> Moves { get; set; } = new();
        public int JosekiEndIndex { get; set; } = -1;

        public bool ShowVariation { get; set; } = false;
    }

}
