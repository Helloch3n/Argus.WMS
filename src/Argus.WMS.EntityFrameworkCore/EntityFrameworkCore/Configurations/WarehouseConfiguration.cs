using Argus.WMS.MasterData.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("AppWarehouses");

            builder.ConfigureByConvention();

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("²Ö¿â±àÂë");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("²Ö¿âÃû³Æ");

            builder.Property(x => x.Address)
                .HasMaxLength(500)
                .HasComment("µØÖ·");

            builder.Property(x => x.Manager)
                .HasMaxLength(50)
                .HasComment("¸ºÔðÈË");

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasMany(x => x.Zones)
                .WithOne()
                .HasForeignKey(x => x.WarehouseId)
                .IsRequired();
        }
    }
}