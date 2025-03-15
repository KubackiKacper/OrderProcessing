using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Models;

namespace OrderProcessing
{
    public interface IOrderProcessing
    {
        public Order[] GetOrders();
        
    }
}
