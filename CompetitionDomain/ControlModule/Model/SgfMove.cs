using System;
using System.Collections.Generic;
using System.Text;

namespace CompetitionDomain.ControlModule.Model
{
    public class SgfMove
    {
        public int Move { get; set; }
        public string Color { get; set; } = "";
        public string Sgf { get; set; } = "";
        public int? X { get; set; }
        public int? Y { get; set; }
    }
}
