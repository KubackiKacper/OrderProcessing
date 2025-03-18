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
            .AddSingleton<IOrderProcessing, OrderProcessing>()
            .BuildServiceProvider();

            var orderProcessing = serviceProvider.GetService<IOrderProcessing>();

            string userInput = "";
            Console.WriteLine("Welcome to order processing console app!");
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Place new order.");
            Console.WriteLine("2. Change order status.");
            Console.WriteLine("3. Display orders.");
            Console.WriteLine("4. Exit.");
            do
            {
                userInput = Console.ReadLine();
                int validationOfUserinput = Validate.ValidateUserInput(userInput);

                switch (validationOfUserinput)
                {
                    case 1:
                        //Console.WriteLine("work in progress");
                        await orderProcessing.PlaceNewOrder();
                        break;

                    case 2:
                        //Console.WriteLine("work in progress");
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
