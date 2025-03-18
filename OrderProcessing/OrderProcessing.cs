using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProcessing.Data;
using OrderProcessing.Models;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Storage.Json;
namespace OrderProcessing
{
    public class OrderProcessing : IOrderProcessing
    {
        private readonly ApplicationDbContext _context;        
        public OrderProcessing(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetOrderDTO[]> GetOrders() 
        {
            Console.Clear();
            GetOrderDTO[] response = await _context.Orders
                .Select(order => new GetOrderDTO
                {
                    Id = order.Id,
                    TotalOfOrder = order.TotalOfOrder,
                    NameOfProducts = string.Join(", ", order.OrdersProducts.Select(op => op.Product.ProductName)),
                    TypeOfClient = order.TypeOfClient,
                    Address = order.Address,
                    TypeOfPayment = order.TypeOfPayment,
                    Statuses = order.Statuses.ToList()
                }).ToArrayAsync();

                foreach (var order in response)
                {
                    Console.WriteLine($"Order ID: {order.Id},\nProducts: {order.NameOfProducts},\nType Of Client: {order.TypeOfClient}\n" +
                                        $"Address: {order.Address},\nType Of Payment: {order.TypeOfPayment}\n" +
                                        $"Total Of order: {order.TotalOfOrder}");

                    if (order.Statuses != null && order.Statuses.Any())
                    {
                        foreach (var status in order.Statuses)
                        {
                            Console.WriteLine($"Status: {status.Status}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No status available!");
                    }
                    Console.WriteLine();
                }
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Place new order.");
            Console.WriteLine("2. Change order status.");
            Console.WriteLine("3. Display orders.");
            Console.WriteLine("4. Exit.");
            return response;
        }  
        public async Task<GetProductsDTO[]> GetProducts()
        {
            GetProductsDTO[] response = await _context.Products
                .Select(product => new GetProductsDTO
                {
                    Id = product.Id,
                    UnitPrice = product.UnitPrice,
                    ProductName = product.ProductName
                }).ToArrayAsync();
            foreach (var product in response)
            {
                Console.WriteLine($"{product.Id}. Name: {product.ProductName}, Price:{product.UnitPrice}");
            }
            return response;
        }
        public async Task<PlaceOrderDTO> PlaceNewOrder()
        {
            Console.Clear();
            Console.WriteLine("Specify, which product would You like to order");

            await GetProducts();
            string userChoice;
            Product product;
            userChoice = Console.ReadLine();
            int validationOfUserChoice = Validate.ValidateUserInput(userChoice);
            do
            {
                product = await _context.Products.FirstOrDefaultAsync(product => product.Id == validationOfUserChoice);
                if (product == null)
                {
                    Console.WriteLine("Product not found. Please try again.");
                    userChoice= Console.ReadLine();
                }
                else
                {
                    continue;
                }
            } while (product == null);

            string quantity;
            Console.WriteLine("Provide quantity number:");
            quantity = Console.ReadLine();
            int validationOfQuantity = Validate.ValidateUserInput(quantity);

            string address;
            Console.WriteLine("Please provide shipping address:");
            address = Console.ReadLine();
            while (string.IsNullOrEmpty(address))
            {
                Console.WriteLine("Address is required for proper shipment!");
                address = Console.ReadLine();                
            };

            Dictionary<int, string> paymentOptions = new Dictionary<int, string>
            {
                { 1, "Card" },
                { 2, "Cash on delivery" }
            };
            int selectedPayment = Validate.ValidateChoice("What is your payment method?", paymentOptions);
            string typeOfPayment = paymentOptions[selectedPayment];

            Dictionary<int, string> clientType = new Dictionary<int, string>
            {
                { 1, "Individual" },
                { 2, "Buisness" }
            };
            int selectedTypeOfClient = Validate.ValidateChoice("What type of client are You?", clientType);
            string typeOfClient = clientType[selectedTypeOfClient];

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    PlaceOrderDTO placeOrderDTO = new PlaceOrderDTO
                    {
                        TypeOfClient = typeOfClient,
                        Address = address,
                        TypeOfPayment = typeOfPayment
                    };
                    Order order = new Order
                    {
                        TypeOfClient = placeOrderDTO.TypeOfClient,
                        Address = placeOrderDTO.Address,
                        TypeOfPayment = placeOrderDTO.TypeOfPayment
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    OrderProductDTO orderProductDTO = new OrderProductDTO
                    {
                        ProductId = product.Id,
                        OrderId = order.Id,
                        Quantity = validationOfQuantity
                    };

                    OrderProduct orderProduct = new OrderProduct
                    {
                        ProductId = orderProductDTO.ProductId,
                        OrderId = orderProductDTO.OrderId,
                        Quantity = orderProductDTO.Quantity
                    };
                    _context.OrdersProducts.Add(orderProduct);
                    await _context.SaveChangesAsync();

                    OrderStatusDTO orderStatusDTO = new OrderStatusDTO
                    {
                        OrderId = order.Id,
                        Status = "New"
                    };
                    OrderStatus orderStatus = new OrderStatus
                    {
                        OrderId = orderStatusDTO.OrderId,
                        Status = orderStatusDTO.Status
                    };
                    _context.OrdersStatuses.Add(orderStatus);
                    await _context.SaveChangesAsync();

                    Order updateOrder = await _context.Orders.FindAsync(order.Id);
                    updateOrder.TotalOfOrder = product.UnitPrice * validationOfQuantity;
                    updateOrder.NameOfProducts = string.Join(", ", order.OrdersProducts
                        .Select(op => op.Product.ProductName));
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Order placed successfully!");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
;            }
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Place new order.");
            Console.WriteLine("2. Change order status.");
            Console.WriteLine("3. Display orders.");
            Console.WriteLine("4. Exit.");
            return null;
        }
    }
}
