namespace NotificationService.Models
{
    /// <summary>
    /// Represents a notification related to an order.
    /// </summary>
    public class OrderNotification
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public required string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Gets or sets the device token to which the notification will be sent.
        /// </summary>
        public required string DeviceToken { get; set; }
    }
}
