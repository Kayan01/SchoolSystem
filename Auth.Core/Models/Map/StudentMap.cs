using System;
using System.Collections.Generic;
using System.Text;
using Auth.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Core.Models.Map
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            //builder.HasOne(a => a.TeachingStaff)
            //    .WithOne(b => b.Staff)
            //    .HasForeignKey<TeachingStaff>(b => b.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(b => b.RegNumber)
                .IsUnique();

        }
    }
}
