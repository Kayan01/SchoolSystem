using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Models.Map
{
    public class SchoolClassSubjectMap : IEntityTypeConfiguration<SchoolClassSubject>
    {
        public void Configure(EntityTypeBuilder<SchoolClassSubject> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever();
        }
    }
}
