﻿using Microsoft.EntityFrameworkCore;
using OrderProcessing.Data;
using OrderProcessing.Models;
using Microsoft.Extensions.DependencyInjection;

namespace OrderProcessing
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=OrderProcessing.db")
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableSensitiveDataLogging(true))
            .AddSingleton<IOrderProcessing, OrderProcessingService>()
            .BuildServiceProvider();

            var orderProcessing = serviceProvider.GetService<IOrderProcessing>();

            string userInput = "";
            Console.WriteLine("Welcome to order processing console app!");
            DisplayMenu.ShowMenu();
            do
            {
                userInput = Console.ReadLine();
                int validationOfUserinput = Validate.ValidateUserInput(userInput);

                switch (validationOfUserinput)
                {
                    case 1:
                        await orderProcessing.PlaceNewOrder();
                        break;

                    case 2:
                        await orderProcessing.UpdateOrderStatus();
                        break;

                    case 3:                        
                        await orderProcessing.GetOrders();                       
                        break;

                    case 4:
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice try again!");
                        break;
                }
            } while (true);
        }        
    }
    
}
