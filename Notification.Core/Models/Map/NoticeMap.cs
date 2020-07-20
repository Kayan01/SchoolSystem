using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Core.Models.Map
{
    public class NoticeMap : IEntityTypeConfiguration<Notice>
    {
        public void Configure(EntityTypeBuilder<Notice> builder)
        {
            builder.ToTable("MyNotice");
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Notice> builder)
        {
            List<Notice> dataList = new List<Notice>()
            {
                new Notice
                {
                    Id = 1,
                    Description = "Testing"
                },
                new Notice
                {
                    Id = 2,
                    Description = "Unit Test"

                }
            };

            builder.HasData(dataList);
        }
    }
}