using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.ControlModule.Model;
using CompetitionDomain.Model;
using System.Text.RegularExpressions;
using Match = System.Text.RegularExpressions.Match;

namespace CompetitionDomain.ControlModule.Services
{
    public class SgfParser : ISgfParser
    {
        private static readonly Regex MoveRegex =  new(";\\s*([BW])\\s*\\[([a-s]{0,2})\\]", RegexOptions.Compiled);

        public List<SgfMove> ParseMoves(string sgf)
        {
            // Normalize line endings
            sgf = sgf.Replace("\r", "");

            // Normalize weird Unicode whitespace
            sgf = sgf
                .Replace("\u3000", " ")
                .Replace("\u00A0", " ")
                .Replace("\u2003", " ")
                .Replace("\u200B", "");

            var moves = new List<SgfMove>();
            int moveNumber = 1;

            foreach (Match m in MoveRegex.Matches(sgf))
            {
                string color = m.Groups[1].Value;
                string sgfCoord = m.Groups[2].Value;

                int? x = null;
                int? y = null;

                if (sgfCoord.Length == 2)
                {
                    x = sgfCoord[0] - 'a';
                    y = sgfCoord[1] - 'a';
                }

                moves.Add(new SgfMove
                {
                    Move = moveNumber++,
                    Color = color,
                    Sgf = sgfCoord,
                    X = x,
                    Y = y
                });
            }

            return moves;
        }
    }
}
