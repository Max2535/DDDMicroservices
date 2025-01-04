using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class KafkaProducerService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? throw new ArgumentNullException(nameof(configuration), "Kafka:BootstrapServers is not configured");
            _topic = configuration["Kafka:Topic"] ?? throw new ArgumentNullException(nameof(configuration), "Kafka:Topic is not configured");
        }

        public async Task ProduceAsync(OrderNotification notification)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var message = new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(notification)
            };

            await producer.ProduceAsync(_topic, message);
        }
    }
}
