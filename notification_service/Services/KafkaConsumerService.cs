using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class KafkaConsumerService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        private readonly FirebaseNotificationService _firebaseService;

        public KafkaConsumerService(IConfiguration configuration, FirebaseNotificationService firebaseService)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? throw new ArgumentNullException(nameof(configuration), "Kafka:BootstrapServers configuration is missing");
            _topic = configuration["Kafka:Topic"] ?? throw new ArgumentNullException(nameof(configuration), "Kafka:Topic configuration is missing");
            _firebaseService = firebaseService;
        }

        public async Task StartConsumingAsync()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "notification-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");
                var notification = JsonConvert.DeserializeObject<OrderNotification>(result.Message.Value);
                if (notification != null)
                {
                    await _firebaseService.SendNotificationAsync(notification);
                }
            }
        }
    }
}
