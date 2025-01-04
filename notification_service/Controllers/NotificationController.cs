using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly KafkaProducerService _kafkaProducer;

        public NotificationController(KafkaProducerService kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendNotification([FromBody] OrderNotification notification)
        {
            if (notification == null || string.IsNullOrEmpty(notification.DeviceToken))
            {
                return BadRequest("Invalid notification payload");
            }

            await _kafkaProducer.ProduceAsync(notification);
            return Ok(new { Message = "Notification queued successfully" });
        }
    }
}
