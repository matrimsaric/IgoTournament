using ConsoleApp;
using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using ServerCommonModule.Configuration;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;
using static RepositoryBootstrapper;
//using TournamentDomain.ControlModule;

class Program
{
    static async Task Main(string[] args)
    {
        var bootstrap = new RepositoryBootstrapper(DatabaseEnvironment.Test);

        var playerRepo = bootstrap.CreatePlayerRepository();
        var roundRepo = bootstrap.CreateRoundRepository();
        var matchRepo = bootstrap.CreateMatchRepository();
        var sgfRepo = bootstrap.CreateSgfRepository();

        var playerHandler = new PlayerConsoleHandler(playerRepo);

        var router = new TaskRouter(
            playerHandler,
            playerRepo,
            roundRepo,
            matchRepo,
            sgfRepo
        );


        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("2. Match Wizard");
            Console.WriteLine("1. Player Management");
            Console.WriteLine("0. Exit");
            Console.Write("Select a task: ");

            var input = Console.ReadLine();

            if (input == "0")
            {
                exit = true;
            }
            else if (int.TryParse(input, out int taskNumber))
            {
                await router.Execute(taskNumber);
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}
