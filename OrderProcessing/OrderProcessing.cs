using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Migrations;
using OrderProcessing.Data;
using OrderProcessing.Models;
namespace OrderProcessing
{
    public class OrderProcessing
    {
        private readonly ApplicationDbContext _context;
        public OrderProcessing(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order AddOrder ()
        {
            Order add = new Order
            {
                Id = 1,
                TotalOfOrder = 10,
                NameOfProduct = "test",
                TypeOfClient = "test",
                Address = "test",
                TypeOfPayment = "test"
            };
            _context.Orders.Add(add);
            _context.SaveChanges();
            return add;
        }
    }
}
