using System;
using System.Collections.Generic;
using System.Text;
using CompetitionDomain.ControlModule.Model;
using CompetitionDomain.Model;

namespace CompetitionDomain.ControlModule.Interfaces
{
    public interface ISgfParser
    {
        List<SgfMove> ParseMoves(string sgf);
    }
}
