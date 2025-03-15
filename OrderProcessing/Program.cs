using Microsoft.EntityFrameworkCore;
using OrderProcessing.Data;
using OrderProcessing.Models;

namespace OrderProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new ApplicationDbContext();
            
            var orderProcessing = new OrderProcessing(context);
            var orders = orderProcessing.GetOrders();
            foreach (var order in orders)
            {
                Console.WriteLine($"Zamówienie ID: {order.Id},\nProdukt: {order.NameOfProduct}");

                if (order.Statuses != null && order.Statuses.Any())
                {
                    foreach (var status in order.Statuses)
                    {
                        Console.WriteLine($"- Status: {status.Status},");
                    }
                }
                else
                {
                    Console.WriteLine("- Brak statusów dla tego zamówienia.");
                }
                Console.WriteLine();
            }

        }
    }
}
