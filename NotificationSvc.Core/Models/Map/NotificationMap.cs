using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSvc.Core.Models.Map
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Notification> builder)
        {
            List<Notification> dataList = new List<Notification>()
            {
                new Notification
                {
                    Id = 1,
                    Description = "Testing"
                },
                new Notification
                {
                    Id = 2,
                    Description = "Unit Test"

                }
            };

            builder.HasData(dataList);
        }
    }
}