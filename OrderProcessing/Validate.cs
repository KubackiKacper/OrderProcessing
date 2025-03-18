using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    public static class Validate
    {
        public static int ValidateUserInput(string userInput)
        {
            while (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Please enter your choice!");
                userInput = Console.ReadLine();
            }

            if (!int.TryParse(userInput, out int choice))
            {
                Console.WriteLine("Invalid input, please enter a number.");
                userInput = Console.ReadLine();
            }
            return choice;
        }

        public static int ValidateChoice(string message, Dictionary<int, string> options)
        {
            int choice;
            string userInput;
            do
            {
                Console.WriteLine(message);
                foreach (var option in options)
                {
                    Console.WriteLine($"{option.Key}. {option.Value}");
                }

                userInput = Console.ReadLine();
            }
            while (!int.TryParse(userInput, out choice) || !options.ContainsKey(choice));

            return choice;
        }
    }
}
