using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Map
{
    public class TestMap : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("MyTest");
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Test> builder)
        {
            List<Test> dataList = new List<Test>()
            {
                new Test
                {
                    Id = 1,
                    Description = "Testing",
                    Title = "Debug"
                },
                new Test
                {
                    Id = 2,
                    Description = "Unit Test",
                    Title = "Test"

                }
            };

            builder.HasData(dataList);
        }
    }
}
