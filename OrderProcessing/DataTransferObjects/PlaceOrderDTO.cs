using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Models;

namespace OrderProcessing.DataTransferObjects
{
    public class PlaceOrderDTO
    {
        [MaxLength(255)]
        public string? NameOfProducts { get; set; }

        [MaxLength(255)]
        public string TypeOfClient { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string TypeOfPayment { get; set; }
        public ICollection<OrderStatus> Statuses { get; set; } = new List<OrderStatus>();
    }
}
