using Auth.Core.Models.JoinTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Core.Models.Map
{
    public class ClassGroupJoinMap : IEntityTypeConfiguration<Class2Group>

    {
        /// <summary>
        /// define a composite key
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void Configure(EntityTypeBuilder<Class2Group> modelBuilder)
        {
            
            modelBuilder.HasKey(bc => new { bc.ClassGroupId, bc.SchoolClassId });
            modelBuilder.HasOne(bc => bc.ClassGroup)
                .WithMany(b => b.ClassJoinGroup)
                .HasForeignKey(bc => bc.ClassGroupId);
            modelBuilder.HasOne(bc => bc.SchoolClass)
                .WithMany(c => c.ClassJoinGroup)
                .HasForeignKey(bc => bc.SchoolClassId);
        }
    }
}
