using CompetitionDomain.ControlModule.Interfaces;
using CompetitionDomain.Model;
using ConsoleApp;
using PlayerDomain.ControlModule.Interfaces;
using PlayerDomain.Model;
using System;
using System.Text;
using System.Text.RegularExpressions;

using Match = CompetitionDomain.Model.Match;

public class MatchEntryWizard
{
    private readonly IRoundRepository roundRepo;
    private readonly IPlayerRepository playerRepo;
    private readonly IMatchRepository matchRepo;
    private readonly ISgfRecordRepository sgfRepo;

    public MatchEntryWizard(
        IRoundRepository roundRepo,
        IPlayerRepository playerRepo,
        IMatchRepository matchRepo,
        ISgfRecordRepository sgfRepo)
    {
        this.roundRepo = roundRepo;
        this.playerRepo = playerRepo;
        this.matchRepo = matchRepo;
        this.sgfRepo = sgfRepo;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("\n=== Match Entry Wizard ===");

        var round = await SelectRound();
        var boardNumber = AskInt("Enter board number");

        var black = await SelectPlayer("Select BLACK player");
        var white = await SelectPlayer("Select WHITE player");

        Console.Write("Enter result (e.g., B+R, W+5.5): ");
        string result = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter SGF file path: ");
        string sgfPath = Console.ReadLine() ?? string.Empty;

        var cleanPath = sgfPath
    .Trim()                     // removes whitespace, \r, \n
    .Trim('"')                  // removes leading/trailing quotes
    .Replace("\"", "")          // removes any remaining quotes inside
    .Replace("\u0000", "");     // removes null chars if present



        if (!File.Exists(cleanPath))
        {
            Console.WriteLine("SGF file not found.");
            return;
        }

        string rawSgf = File.ReadAllText(cleanPath);
        string parsedJson = SgfParser.ParseMovesToJson(rawSgf);

        var match = new Match
        {
            Id = Guid.NewGuid(),
            Name = $"{black.Name} vs {white.Name}",
            RoundId = round.Id,
            BoardNumber = boardNumber,
            BlackPlayerId = black.Id,
            WhitePlayerId = white.Id,
            Result = result,
            WinnerId = DetermineWinner(result, black, white),
            GameDate = DateTime.Today
        };

        Console.WriteLine("\n=== Confirm Match Entry ===");
        Console.WriteLine($"Round: {round.Name} (#{round.RoundNumber})");
        Console.WriteLine($"Board: {boardNumber}");
        Console.WriteLine($"Black: {black.Name}");
        Console.WriteLine($"White: {white.Name}");
        Console.WriteLine($"Result: {result}");
        Console.WriteLine($"SGF file: {sgfPath}");

        if (!ConsoleHelpers.Confirm("Save this match"))
        {
            Console.WriteLine("Match entry cancelled.");
            return;
        }


        await matchRepo.CreateMatch(match);

        string autoName = SgfRecord.GenerateAutoName(match.GameDate, round.RoundNumber, boardNumber, black.Name, white.Name);

        var sgfRecord = new SgfRecord
        {
            Id = Guid.NewGuid(),
            Name = autoName,
            MatchId = match.Id,
            RawSgf = rawSgf,
            ParsedMovesJson = parsedJson,
            RetrievedAt = DateTime.Now
        };

        await sgfRepo.CreateSgfRecord(sgfRecord);

        match.SgfId = sgfRecord.Id;
        await matchRepo.UpdateMatch(match);

        Console.WriteLine("\nMatch and SGF successfully saved.");
    }

    private async Task<Round> SelectRound()
    {
        var rounds = await roundRepo.GetAllRounds();
        return ConsoleHelpers.SelectFromCollection(rounds, r => $"{r.Name} (Round {r.RoundNumber})", "Select a round");
    }


    private async Task<Player> SelectPlayer(string prompt)
    {
        var players = await playerRepo.GetAllPlayers();
        return ConsoleHelpers.SelectFromCollection(players, p => $"{p.Name} ({p.Rank})", prompt);
    }


    private int AskInt(string prompt)
    {
        Console.Write($"{prompt}: ");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private Guid? DetermineWinner(string result, Player black, Player white)
    {
        if (string.IsNullOrWhiteSpace(result))
            return null;

        if (result.StartsWith("B", StringComparison.OrdinalIgnoreCase))
            return black.Id;

        if (result.StartsWith("W", StringComparison.OrdinalIgnoreCase))
            return white.Id;

        return null;
    }

}
