using Shared.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.Models
{
    public class Notification : CreationAuditedEntity<long>
    {
        public string Description { get; set; }
        public long? EntityId { get; set; }
        public string Entity { get; set; } //Such as Student, Teacher, User, Class, Subject, Timetable etc

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
