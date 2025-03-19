using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using OrderProcessing;
using OrderProcessing.Data;
using OrderProcessing.Models;
using OrderProcessing.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;

public class OrderProcessingTests
{
    private readonly ApplicationDbContext _context;
    private readonly IOrderProcessing _orderProcessing;

    public OrderProcessingTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=OrderProcessingTest.db")
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableSensitiveDataLogging(true))
            .AddSingleton<IOrderProcessing, OrderProcessingService>()
            .BuildServiceProvider();
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _orderProcessing = serviceProvider.GetRequiredService<IOrderProcessing>();

        _context.Orders.RemoveRange(_context.Orders);
        _context.Database.ExecuteSqlRaw($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{nameof(_context.Orders)}';");

        _context.OrdersProducts.RemoveRange(_context.OrdersProducts);
        _context.Database.ExecuteSqlRaw($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{nameof(_context.OrdersProducts)}';");

        _context.OrdersStatuses.RemoveRange(_context.OrdersStatuses);
        _context.Database.ExecuteSqlRaw($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{nameof(_context.OrdersStatuses)}';");

        _context.Products.RemoveRange(_context.Products);
        _context.Database.ExecuteSqlRaw($"UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='{nameof(_context.Products)}';");

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _context.Products.AddRange(new List<Product>
        {
            new Product { Id = 1, ProductName = "Laptop", UnitPrice = 5000 },
            new Product { Id = 2, ProductName = "Mouse", UnitPrice = 150 }
        });

        _context.Orders.Add(new Order
        {
            Id = 1,
            TypeOfClient = "Individual",
            Address = "Test Street 123",
            TypeOfPayment = "Card",
            TotalOfOrder = 5150,
            OrdersProducts = new List<OrderProduct>
            {
                new OrderProduct { ProductId = 1, Quantity = 1 },
                new OrderProduct { ProductId = 2, Quantity = 1 }
            },
            Statuses = new List<OrderStatus>
            {
                new OrderStatus { Status = "New" }
            }
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrders()
    {
        var result = await _orderProcessing.GetOrders();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Laptop, Mouse", result[0].NameOfProducts);
        Assert.Equal("New", result[0].Statuses.First().Status);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnProducts()
    {
        var result = await _orderProcessing.GetProducts();

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Contains(result, p => p.ProductName == "Laptop");
        Assert.Contains(result, p => p.ProductName == "Mouse");
    }

    [Fact]
    public async Task PlaceNewOrder_ShouldAddOrder()
    {
        var order = new PlaceOrderDTO
        {
            TypeOfClient = "Business",
            Address = "Test Street 456",
            TypeOfPayment = "Cash on delivery"
        };

        await _orderProcessing.PlaceNewOrder();

        var orders = _context.Orders.ToList();
        Assert.Equal(2, orders.Count);
        Assert.Contains(orders, o => o.Address == "Test Street 456");
    }

    [Fact]
    public async Task UpdateOrderStatus_ShouldUpdateStatus()
    {
        var order = _context.Orders.First();
        var newStatus = new OrderStatus { OrderId = order.Id, Status = "In Delivery" };

        order.Statuses.Add(newStatus);
        await _context.SaveChangesAsync();

        var updatedOrder = _context.Orders.Include(o => o.Statuses).First();
        Assert.Contains(updatedOrder.Statuses, s => s.Status == "In Delivery");
    }
}
