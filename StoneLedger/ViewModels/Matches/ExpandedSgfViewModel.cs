using StoneLedger.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.ViewModels.Matches
{
    public class ExpandedSgfViewModel
    {
        public IList<SgfMove> Moves { get; }

        public ExpandedSgfViewModel(IList<SgfMove> moves)
        {
            Moves = moves;
        }
    }

}
