using Argus.WMS.Inventorys;
using Argus.WMS.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.ToTable("AppInventoryTransactions");

            builder.ConfigureByConvention();

            builder.HasIndex(x => x.BillNo);
            builder.HasIndex(x => x.InventoryId);
            builder.HasIndex(x => x.CreationTime);

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.Product.IsDeleted);

            builder.HasOne(x => x.FromLocation)
    .WithMany()
    .HasForeignKey(x => x.FromLocationId)
    .OnDelete(DeleteBehavior.Restrict); // ·ÀÖ¹¼¶ÁªÉ¾³ý

            builder.HasOne(x => x.ToLocation)
                .WithMany()
                .HasForeignKey(x => x.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FromWarehouse)
        .WithMany()
        .HasForeignKey(x => x.FromWarehouseId)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ToWarehouse)
                .WithMany()
                .HasForeignKey(x => x.ToWarehouseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}