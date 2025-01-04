using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddSingleton<FirebaseNotificationService>();
builder.Services.AddSingleton<KafkaConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map Controllers
app.MapControllers();

var kafkaConsumer = app.Services.GetRequiredService<KafkaConsumerService>();
Task.Run(() => kafkaConsumer.StartConsumingAsync());

app.Run();

