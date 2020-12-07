using AssessmentSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningSvc.Core.Models.Map
{
    public class TeacherMap : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever();
        }
    }
}
