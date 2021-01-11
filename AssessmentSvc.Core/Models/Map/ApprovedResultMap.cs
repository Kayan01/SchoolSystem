using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models.Map
{
    public class ApprovedResultMap : IEntityTypeConfiguration<ApprovedResult>
    {
        public void Configure(EntityTypeBuilder<ApprovedResult> builder)
        {
            builder.HasMany(x => x.Results)
                .WithOne(x => x.ApprovedResult)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
