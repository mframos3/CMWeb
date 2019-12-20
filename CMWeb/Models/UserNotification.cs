using CMWeb.Areas.Identity.Data;

namespace CMWeb.Models
{
    public class UserNotification
    {
        public string UserId { get; set; }
        public CMWebUser User { get; set; }

        public string NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}