using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class ConsoleHelpers
    {
        public static T SelectFromCollection<T>(
            IEnumerable<T> items,
            Func<T, string> label,
            string prompt)
        {
            var list = items.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("No items available.");
                throw new InvalidOperationException("Cannot select from an empty collection.");
            }

            Console.WriteLine($"\n{prompt}:");

            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{i + 1}. {label(list[i])}");

            while (true)
            {
                Console.Write("Enter number: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int index))
                {
                    index -= 1; // convert to zero‑based

                    if (index >= 0 && index < list.Count)
                        return list[index];
                }

                Console.WriteLine("Invalid selection. Please enter a valid number.");
            }
        }

        public static bool Confirm(string message)
        {
            while (true)
            {
                Console.Write($"{message} (Y/N): ");
                var input = Console.ReadLine()?.Trim().ToUpperInvariant();

                if (input == "Y") return true;
                if (input == "N") return false;

                Console.WriteLine("Please enter Y or N.");
            }
        }
    }
}
