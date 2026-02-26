using Argus.WMS.MasterData.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.ToTable("AppZones");

            builder.ConfigureByConvention();

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("库区编码");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("库区名称");

            builder.Property(x => x.ZoneType)
                .HasComment("库区类型");

            builder.HasIndex(x => x.WarehouseId);
        }
    }
}