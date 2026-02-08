using CompetitionDomain.ControlModule;
using CompetitionDomain.ControlModule.Interfaces;
using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using ServerCommonModule.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleApp
{
    public class TaskRouter
    {
        private readonly PlayerConsoleHandler _playerHandler;
        private readonly IRoundRepository _roundRepo;
        private readonly IPlayerRepository _playerRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly ISgfRecordRepository _sgfRepo;

        public TaskRouter(
            PlayerConsoleHandler playerHandler,
            IPlayerRepository playerRepo,
            IRoundRepository roundRepo,
            IMatchRepository matchRepo,
            ISgfRecordRepository sgfRepo)
        {
            _playerHandler = playerHandler;
            _playerRepo = playerRepo;
            _roundRepo = roundRepo;
            _matchRepo = matchRepo;
            _sgfRepo = sgfRepo;
        }

        public async Task Execute(int taskNumber)
        {
            switch (taskNumber)
            {
                case 1:
                    await _playerHandler.Run();
                    break;
                case 2:
                    
                    var wizard = new MatchEntryWizard(
                        _roundRepo,
                        _playerRepo,
                        _matchRepo,
                        _sgfRepo
                    );

                    await wizard.RunAsync();
                    break;
                default:
                    Console.WriteLine("Unknown task.");
                    break;
            }
        }
    }
}