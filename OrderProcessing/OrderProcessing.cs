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

            List<OrderProduct> orderProducts = new List<OrderProduct>();
            Dictionary<int, int> productQuantities = new Dictionary<int, int>();
            List<string> orderedProductNames = new List<string>();

            string addMoreProducts;
            do
            {
                await GetProducts();
                string userChoice;
                Product product;
                do
                {
                    userChoice = Console.ReadLine();
                    int validationOfUserChoice = Validate.ValidateUserInput(userChoice);
                    product = await _context.Products.FirstOrDefaultAsync(p => p.Id == validationOfUserChoice);
                    if (product == null)
                    {
                        Console.WriteLine("Product not found. Please try again.");
                    }
                } while (product == null);

                Console.WriteLine("Provide quantity number:");
                string quantity = Console.ReadLine();
                int validationOfQuantity = Validate.ValidateUserInput(quantity);
                if (productQuantities.ContainsKey(product.Id))
                {
                    productQuantities[product.Id] += validationOfQuantity;
                }
                else
                {
                    productQuantities.Add(product.Id, validationOfQuantity);
                }

                Console.WriteLine("Would you like to add another product? (yes/no)");
                addMoreProducts = Console.ReadLine()?.Trim().ToLower();
            } while (addMoreProducts == "yes");

            Console.WriteLine("Please provide shipping address:");
            string address;
            do
            {
                address = Console.ReadLine();
                if (string.IsNullOrEmpty(address))
                {
                    Console.WriteLine("Address is required for proper shipment!");
                }
            } while (string.IsNullOrEmpty(address));

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
                { 2, "Business" }
            };
            int selectedTypeOfClient = Validate.ValidateChoice("What type of client are You?", clientType);
            string typeOfClient = clientType[selectedTypeOfClient];

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Order order = new Order
                    {
                        TypeOfClient = typeOfClient,
                        Address = address,
                        TypeOfPayment = typeOfPayment
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    decimal totalOrderPrice = 0;
                    foreach (var kvp in productQuantities)
                    {
                        int productId = kvp.Key;
                        int quantity = kvp.Value;

                        Product product = await _context.Products.FindAsync(productId);
                        if (product != null)
                        {
                            totalOrderPrice += product.UnitPrice * quantity;
                            orderedProductNames.Add($"{product.ProductName} (x{quantity})");

                            OrderProduct existingOrderProduct = await _context.OrdersProducts
                                .FirstOrDefaultAsync(op => op.OrderId == order.Id && op.ProductId == productId);

                            if (existingOrderProduct != null)
                            {
                                existingOrderProduct.Quantity += quantity;
                            }
                            else
                            {
                                _context.OrdersProducts.Add(new OrderProduct
                                {
                                    OrderId = order.Id,
                                    ProductId = productId,
                                    Quantity = quantity
                                });
                            }
                        }
                    }
                    await _context.SaveChangesAsync();

                    OrderStatus orderStatus = new OrderStatus
                    {
                        OrderId = order.Id,
                        Status = "New"
                    };
                    _context.OrdersStatuses.Add(orderStatus);
                    await _context.SaveChangesAsync();

                    order.TotalOfOrder = totalOrderPrice;
                    order.NameOfProducts = string.Join(", ", orderedProductNames);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine("Order placed successfully!\n");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1. Place new order.");
            Console.WriteLine("2. Change order status.");
            Console.WriteLine("3. Display orders.");
            Console.WriteLine("4. Exit.");
            return null;
        }

    }
}
