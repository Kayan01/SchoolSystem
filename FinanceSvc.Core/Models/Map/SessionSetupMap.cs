using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSvc.Core.Models.Map
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
