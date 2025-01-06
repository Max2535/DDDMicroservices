using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using inventory_management.Data;
using inventory_management.Models;

namespace InventorySystem.Services
{
    public class RabbitMqConsumer
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "SALE_PRODUCT", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var saleOrder = JsonSerializer.Deserialize<SaleOrderDto>(message);
                if (saleOrder != null)
                {
                    await ProcessStock(saleOrder);
                }

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: "SALE_PRODUCT", autoAck: false, consumer: consumer);

            Console.WriteLine("Listening to SALE_PRODUCT queue...");
            Console.ReadLine();
        }

        private async Task ProcessStock(SaleOrderDto saleOrder)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventDbContext>();

            foreach (var item in saleOrder.Items)
            {
                var stockBalance = await dbContext.stock_balances.FirstOrDefaultAsync(s => s.product_id == item.ProductId);
                if (stockBalance == null || stockBalance.quantity < item.Quantity)
                {
                    Console.WriteLine($"Insufficient stock for Product ID {item.ProductId}");
                    continue;
                }

                // Reduce stock
                stockBalance.quantity -= item.Quantity;
                stockBalance.updated_at = DateTime.Now;

                // Add stock movement
                var stockMovement = new stock_movement
                {
                    product_id = item.ProductId,
                    movement_type = "Outbound",
                    quantity = item.Quantity,
                    created_at = DateTime.Now
                };

                dbContext.stock_movements.Add(stockMovement);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    public class SaleOrderDto
    {
        public int OrderId { get; set; }
        public List<SaleOrderItemDto> Items { get; set; }
    }

    public class SaleOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
