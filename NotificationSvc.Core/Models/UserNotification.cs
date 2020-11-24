using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.Models
{
    public class UserNotification
    {
        public long NotificationId { get; set; }
        public long UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        public Notification Notification { get; set; }
    }
}
