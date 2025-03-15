using Microsoft.EntityFrameworkCore;
using OrderProcessing.Data;
using OrderProcessing.Models;

namespace OrderProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new ApplicationDbContext(); // Tworzenie DbContext
            if (context.Database.CanConnect())
            {
                Console.WriteLine("Połączenie z bazą danych działa.");
            }
            else
            {
                Console.WriteLine("Brak połączenia z bazą.");
            }
            var orderProcessing = new OrderProcessing(context); // Przekazanie kontekstu
            var order = orderProcessing.AddOrder(); // Dodanie zamówienia
        }
    }
}
