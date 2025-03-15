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
        public decimal TotalOfOrder { get; set; }
        public string NameOfProduct { get; set; }
        public string TypeOfClient { get; set; }
        public string Address { get; set; }
        public string TypeOfPayment { get; set; }
        public ICollection<OrderStatus> Statuses { get; set; } = new List<OrderStatus>();
    }
}
