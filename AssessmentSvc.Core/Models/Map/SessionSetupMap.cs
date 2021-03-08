using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models.Map
{
    public class SessionSetupMap : IEntityTypeConfiguration<SessionSetup>
    {
        public void Configure(EntityTypeBuilder<SessionSetup> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever();
        }
    }
}
