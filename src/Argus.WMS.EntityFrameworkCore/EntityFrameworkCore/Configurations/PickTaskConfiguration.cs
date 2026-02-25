using Argus.WMS.Outbound;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class PickTaskConfiguration : IEntityTypeConfiguration<PickTask>
    {
        public void Configure(EntityTypeBuilder<PickTask> builder)
        {
            builder.ToTable("AppPickTasks");

            builder.ConfigureByConvention();

            builder.Property(x => x.ReelNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("盘具编号");

            builder.Property(x => x.FromLocation)
                .HasMaxLength(50)
                .HasComment("来源库位");

            builder.Property(x => x.ToLocation)
                .HasMaxLength(50)
                .HasComment("目标库位");
        }
    }
}