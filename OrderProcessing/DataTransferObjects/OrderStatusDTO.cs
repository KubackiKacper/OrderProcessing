using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Models;

namespace OrderProcessing.DataTransferObjects
{
    public class OrderStatusDTO
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Status { get; set; }
    }
}
