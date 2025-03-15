using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Data;
using OrderProcessing.Models;
using Microsoft.EntityFrameworkCore;
namespace OrderProcessing
{
    public class OrderProcessing : IOrderProcessing
    {
        private readonly ApplicationDbContext _context;
        public OrderProcessing(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order[] GetOrders() 
        {
            var orders = _context.Orders
                .Include(o => o.Statuses)
                .ToArray();            
            return orders;
        }        
    }
}
