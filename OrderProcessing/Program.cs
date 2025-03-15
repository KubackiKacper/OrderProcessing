using Microsoft.EntityFrameworkCore;
using OrderProcessing.Data;
using OrderProcessing.Models;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;
namespace OrderProcessing
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=OrderProcessing.db"))
            .AddSingleton<IOrderProcessing, OrderProcessing>()
            .BuildServiceProvider();

            var orderProcessing = serviceProvider.GetService<IOrderProcessing>();

            string userInput = "";
            Console.WriteLine("Welcome to order processing console app!");
            do
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Place new order.");
                Console.WriteLine("2. Change order status.");
                Console.WriteLine("3. Display orders.");
                Console.WriteLine("4. Exit.");
                userInput = Console.ReadLine();
                while (userInput == "")
                {
                    Console.WriteLine("Please enter your choice!");
                    userInput = Console.ReadLine();
                };
                int.TryParse(userInput, out int choice);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("work in progress");
                        break;

                    case 2:
                        Console.WriteLine("work in progress");
                        break;

                    case 3:
                        orderProcessing.GetOrders();
                        break;

                    case 4:
                        return;

                    default:
                        Console.WriteLine("Invalid choice try again!");
                        break;
                }
            } while (int.Parse(userInput) != 4);


        }

    }
}
