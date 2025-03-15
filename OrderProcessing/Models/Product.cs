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
        public int Id { get; }
        [MaxLength(255)]
        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public ICollection<Order> Orders = new List<Order>();
    }
}
