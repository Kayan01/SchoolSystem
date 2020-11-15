using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace NotificationSvc.Core.Models.Map
{
    public class UserNotificationMap : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.HasKey(x => new { x.NotificationId, x.UserId });
            builder.HasOne(x => x.Notification);
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<UserNotification> builder)
        {
            List<UserNotification> dataList = new List<UserNotification>()
            {
                new UserNotification
                {
                    UserId = 1,
                    NotificationId = 1,
                    IsRead = true,
                    DateRead = DateTime.Now
                },
                new UserNotification
                {
                    UserId = 1,
                    NotificationId = 2
                }
            };

            builder.HasData(dataList);
        }
    }
}
