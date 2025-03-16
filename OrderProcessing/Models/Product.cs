using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Models
{
    public class Product
    {
        
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public ICollection<OrderProduct> OrdersProducts = new List<OrderProduct>();
    }
}
