using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Data;
using OrderProcessing.Models;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.DataTransferObjects;
namespace OrderProcessing
{
    public class OrderProcessing : IOrderProcessing
    {
        private readonly ApplicationDbContext _context;        
        public OrderProcessing(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetOrderDTO[]> GetOrders() 
        {
            GetOrderDTO[] response = await _context.Orders
                .Select(order => new GetOrderDTO
                {
                    Id = order.Id,
                    TotalOfOrder = order.TotalOfOrder,
                    NameOfProducts = string.Join(", ", order.OrdersProducts.Select(op => op.Product.ProductName)), // Pobieranie nazw produktów
                    TypeOfClient = order.TypeOfClient,
                    Address = order.Address,
                    TypeOfPayment = order.TypeOfPayment,
                    Statuses = order.Statuses.ToList()
                }).ToArrayAsync();

                foreach (var order in response)
                {
                    Console.WriteLine($"Order ID: {order.Id},\nProducts: {order.NameOfProducts},\nType Of Client: {order.TypeOfClient}\n" +
                                        $"Address: {order.Address},\nType Of Payment: {order.TypeOfPayment}");

                    if (order.Statuses != null && order.Statuses.Any())
                    {
                        foreach (var status in order.Statuses)
                        {
                            Console.WriteLine($"Status: {status.Status}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No status available!");
                    }
                    Console.WriteLine();
                }
                return response;
        }  
        public async Task<GetProductsDTO[]> GetProducts()
        {
            GetProductsDTO[] response = await _context.Products
                .Select(product => new GetProductsDTO
                {
                    Id = product.Id,
                    UnitPrice = product.UnitPrice,
                    ProductName = product.ProductName
                }).ToArrayAsync();
            foreach (var product in response)
            {
                Console.WriteLine($"{product.Id}. Name: {product.ProductName}, Price:{product.UnitPrice}");
            }
            return response;
        }
        public async Task<PlaceOrderDTO> PlaceNewOrder()
        {
            Console.Clear();
            Console.WriteLine("Specify, witch product would You like to order");

            await GetProducts();
            string userChoice;
            Product product;
            userChoice = Console.ReadLine();
            while (string.IsNullOrEmpty(userChoice))
            {
                Console.WriteLine("Invalid product, please select again");
                userChoice = Console.ReadLine();
            }
            if (!int.TryParse(userChoice, out int choice))
            {
                Console.WriteLine("Invalid input, please enter a number.");
                userChoice = Console.ReadLine();
            }
            do
            {
                product = await _context.Products.FirstOrDefaultAsync(product => product.Id == choice);
                if (product == null)
                {
                    Console.WriteLine("Product not found. Please try again.");
                    userChoice= Console.ReadLine();
                }
                else
                {
                    continue;
                }
            } while (product == null);

            string quantity;
            Console.WriteLine("Provide quantity number:");
            quantity = Console.ReadLine();
            while (string.IsNullOrEmpty(quantity))
            {
                Console.WriteLine("Invalid product, please select again");
                quantity = Console.ReadLine();
            }
            if (!int.TryParse(userChoice, out int userQuantity))
            {
                Console.WriteLine("Invalid input, please enter a number.");
                quantity = Console.ReadLine();
            }

            return null;
        }
    }
}
