using Auth.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Map
{
    public class ParentMap : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> model)
        {
            model.HasMany(b => b.Students).WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
