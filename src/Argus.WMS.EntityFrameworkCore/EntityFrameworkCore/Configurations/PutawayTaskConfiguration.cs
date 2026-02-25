using Argus.WMS.Putaway;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class PutawayTaskConfiguration : IEntityTypeConfiguration<PutawayTask>
    {
        public void Configure(EntityTypeBuilder<PutawayTask> builder)
        {
            builder.ToTable("AppPutawayTasks");

            builder.ConfigureByConvention();

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.ToLocationCode)
        .IsRequired(false);
        }
    }
}