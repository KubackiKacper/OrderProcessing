using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public decimal TotalOfOrder { get; set; }

        [MaxLength(255)]
        [Required]
        public string NameOfProducts { get; set; }

        [MaxLength(255)]
        [Required]
        public string TypeOfClient { get; set; }

        [MaxLength(255)]
        [Required]
        public string Address { get; set; }

        [MaxLength(255)]
        [Required]
        public string TypeOfPayment { get; set; }
        public ICollection<OrderStatus> Statuses { get; set; } = new List<OrderStatus>();      
        public ICollection<OrderProduct> OrdersProducts = new List<OrderProduct>();
    }
}
