using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

public static class SgfParser
{
    private static readonly Regex MoveRegex = new(";\\s*([BW])\\s*\\[([a-s]{0,2})\\]", RegexOptions.Compiled);

    public static string ParseMovesToJson(string sgf)
    {
        string t1 = MoveRegex.ToString();
        // Normalize line endings
        sgf = sgf.Replace("\r", "");

        // Normalize weird Unicode whitespace
        sgf = sgf
            .Replace("\u3000", " ")
            .Replace("\u00A0", " ")
            .Replace("\u2003", " ")
            .Replace("\u200B", "");

            var moves = new List<object>();
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

            moves.Add(new
            {
                move = moveNumber++,
                color = color,
                sgf = sgfCoord,
                x = x,
                y = y
            });
        }
        File.WriteAllText("C:\\Users\\matri\\OneDrive\\Pictures\\GOPROS\\GameSGFs\\moves.json", JsonSerializer.Serialize(moves), Encoding.UTF8);
        return JsonSerializer.Serialize(moves);
    }
}
