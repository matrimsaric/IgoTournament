using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using PlayerDomain.Model;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ConsoleApp
{


    public class PlayerConsoleHandler
    {
        private readonly PlayerRepository _playerRepository;

        public PlayerConsoleHandler(PlayerRepository playerManager)
        {
            _playerRepository = playerManager;
        }

        public async Task Run()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Player Management ---");
                Console.WriteLine("1. List Players");
                Console.WriteLine("2. Add Player");
                Console.WriteLine("3. Edit Player");
                Console.WriteLine("4. Delete Player");
                Console.WriteLine("0. Back");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ListPlayers();
                        break;

                    case "2":
                        await AddPlayer();
                        break;

                    case "3":
                        await EditPlayer();
                        break;

                    case "4":
                        await DeletePlayer();
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private async Task ListPlayers()
        {
            var players = await _playerRepository.GetAllPlayers(true);

            Console.WriteLine("\nPlayers:");
            foreach (var p in players)
            {
                Console.WriteLine($"{p.Id} | {p.Name} | Rank {p.Rank}");
            }
        }

        private async Task AddPlayer()
        {
            Console.Write("Enter player name: ");
            var name = Console.ReadLine();

            Console.Write("Enter rank: ");
            var rankInput = Console.ReadLine();
           // int.TryParse(rankInput, out int rank);

            var player = new Player
            {
                Id = Guid.NewGuid(),
                Name = name,
                Rank = rankInput
            };

            var status = await _playerRepository.CreatePlayer(player);

            if (string.IsNullOrEmpty(status))
                Console.WriteLine("Player added.");
            else
                Console.WriteLine($"Error: {status}");
        }

        private async Task EditPlayer()
        {
            Console.Write("Enter player ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var player = await _playerRepository.GetPlayerById(id);
            if (player == null)
            {
                Console.WriteLine("Player not found.");
                return;
            }

            Console.Write($"New name (current: {player.Name}): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                player.Name = newName;

            Console.Write($"New rank (current: {player.Rank}): ");
            var newRankInput = Console.ReadLine();
            if (int.TryParse(newRankInput, out int newRank))
                player.Rank = newRankInput;

            var status = await _playerRepository.UpdatePlayer(player);

            if (string.IsNullOrEmpty(status))
                Console.WriteLine("Player updated.");
            else
                Console.WriteLine($"Error: {status}");
        }

        private async Task DeletePlayer()
        {
            Console.Write("Enter player ID: ");
            var idInput = Console.ReadLine();

            if (!Guid.TryParse(idInput, out Guid id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var player = await _playerRepository.GetPlayerById(id);
            if (player == null)
            {
                Console.WriteLine("Player not found.");
                return;
            }

            await _playerRepository.DeletePlayer(player);
            Console.WriteLine("Player deleted.");
        }
    }
}