using Argus.WMS.Inventorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("AppInventorys");

            builder.ConfigureByConvention();

            builder.Property(x => x.Quantity).HasPrecision(18, 4);
            builder.Property(x => x.LockedQuantity).HasPrecision(18, 4);
            builder.Property(x => x.Weight).HasPrecision(18, 4);
            builder.Property(x => x.Index).HasColumnName("Layer_Index");

            builder.Property(x => x.SN)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CraftVersion)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .HasMaxLength(20)
                .HasConversion<string>()
                .HasDefaultValue(InventoryStatus.Good);

            builder.Property(x => x.BatchNo).HasMaxLength(50);
            builder.Property(x => x.Source_WO).HasMaxLength(50);
            builder.Property(x => x.Unit).HasMaxLength(20);

            builder.HasIndex(x => x.ReelId);
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.SN).IsUnique();
        }
    }
}