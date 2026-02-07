using ConsoleApp;
using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;
//using TournamentDomain.ControlModule;

class Program
{
    static async Task Main(string[] args)
    {
        //string sgf = File.ReadAllText("C:\\Users\\matri\\OneDrive\\Pictures\\GOPROS\\GameSGFs\\match1game1.sgf"); 
        //string json = SgfParser.ParseMovesToJson(sgf); 
        //Console.WriteLine(json);
        //var input2 = Console.ReadLine();
        //return;
        // Set up environment parameters
        IEnvironmentalParameters env = new EnvironmentalParameters();
        env.ConnectionString = "Host=localhost;Username=postgres;Password=modena;Database=IgoTournament";
        env.ConnectionString = "Host=localhost;Port=5434;Username=postgres;Password=modena;Database=IgoTournament";
        env.DatabaseType = "PostgreSQL";

        IDbUtilityFactory dbFactory = new PgUtilityFactory(env, null);

        // Create manager + handler
        var playerManager = new PlayerRepository(env, dbFactory);
        var playerHandler = new PlayerConsoleHandler(playerManager);


        var router = new TaskRouter(playerHandler, env, dbFactory);

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
