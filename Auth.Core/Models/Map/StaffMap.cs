using System;
using System.Collections.Generic;
using System.Text;
using Auth.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Core.Models.Map
{
    public class StaffMap : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            //builder.HasOne(a => a.TeachingStaff)
            //    .WithOne(b => b.Staff)
            //    .HasForeignKey<TeachingStaff>(b => b.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
