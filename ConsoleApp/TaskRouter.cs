using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class TaskRouter
    {
        private readonly PlayerConsoleHandler _playerHandler;

        public TaskRouter(PlayerConsoleHandler playerHandler)
        {
            _playerHandler = playerHandler;
        }

        public async Task Execute(int taskNumber)
        {
            switch (taskNumber)
            {
                case 1:
                    await _playerHandler.Run();
                    break;

                default:
                    Console.WriteLine("Unknown task.");
                    break;
            }
        }
    }
}