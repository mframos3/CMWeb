using System.Collections.Generic;
namespace CMWeb.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}