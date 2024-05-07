using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace OrderService.Managers;

public class OrderItemsManager : IOrderItems
{
    private readonly MyDbContext _context;
    private readonly IModel _channel;

    public OrderItemsManager(MyDbContext context, IConnection connection)
    {
        _context = context;
        _channel = connection.CreateModel(); // Create a channel per instance, or manage lifecycle elsewhere

        // Ensure the queue exists when the manager is instantiated
        _channel.QueueDeclare(queue: "orderitems",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    private void PublishOrderMessage(OrderItem order, string action)
    {
        var messageObject = new
        {
            OrderId = order.OrderItemId,
            TotalAmount = order.Price,
            CustomerId = order.Quantity,
            Action = action  // "Added" or "Updated"
        };

        var messageString = JsonSerializer.Serialize(messageObject);
        var body = Encoding.UTF8.GetBytes(messageString);

        _channel.BasicPublish(exchange: "",
            routingKey: "orderitems",
            basicProperties: null,
            body: body);
    }

    public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
    {
        return await _context.OrderItems.ToListAsync();
    }

    public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
    {
        return await _context.OrderItems.FindAsync(orderItemId);
    }

    public async Task AddOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderItemAsync(OrderItem orderItem)
    {
        _context.Entry(orderItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        PublishOrderMessage(orderItem, "Updated");
    }

    public async Task DeleteOrderItemAsync(int orderItemId)
    {
        var orderItem = await _context.OrderItems.FindAsync(orderItemId);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}