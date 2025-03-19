using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    public static class DisplayMenu
    {
        public static void ShowMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Place new order.");
            Console.WriteLine("2. Change order status.");
            Console.WriteLine("3. Display orders.");
            Console.WriteLine("4. Exit.");
        }
    }
}
