using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Models;

namespace OrderProcessing.DataTransferObjects
{
    public class GetProductsDTO
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public ICollection<OrderProduct> OrdersProducts = new List<OrderProduct>();
    }
}
