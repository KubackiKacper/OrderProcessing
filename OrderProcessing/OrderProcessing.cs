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
            GetOrderDTO[] respone = await _context.Orders.Select(orders => new GetOrderDTO
            {
                Id = orders.Id,
                TotalOfOrder = orders.TotalOfOrder,
                ProductId = orders.ProductId,
                TypeOfClient = orders.TypeOfClient,
                Address = orders.Address,
                TypeOfPayment = orders.TypeOfPayment,
                Statuses = orders.Statuses
            }).ToArrayAsync();

            foreach (var order in respone)
            {
                Console.WriteLine($"Order ID: {order.Id},\nProduct: {order.ProductId},\nType Of Client: {order.TypeOfClient}\n" +
                    $"Address: {order.Address},\nType Of Payment:{order.TypeOfPayment}");

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
            return respone;
        }        
    }
}
