using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Models;

namespace OrderProcessing.DataTransferObjects
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public decimal TotalOfOrder { get; set; }
        public string? NameOfProducts { get; set; }
        public string TypeOfClient { get; set; }
        public string Address { get; set; }
        public string TypeOfPayment { get; set; }
        public ICollection<OrderStatus> Statuses { get; set; } = new List<OrderStatus>();
        public ICollection<OrderProduct> OrdersProducts = new List<OrderProduct>();
    }
}
