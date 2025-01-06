
using inventory_management.Data;
using InventorySystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<InventDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add RabbitMQ Consumer
builder.Services.AddSingleton<RabbitMqConsumer>();

var app = builder.Build();

var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
Task.Run(() => rabbitMqConsumer.StartListening());


app.Run();
