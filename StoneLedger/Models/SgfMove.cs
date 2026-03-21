using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Models
{
    public class SgfMove
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Sgf { get; set; } = "";
        public int Move { get; set; }
        public string Color { get; set; } = "";
    }
}
