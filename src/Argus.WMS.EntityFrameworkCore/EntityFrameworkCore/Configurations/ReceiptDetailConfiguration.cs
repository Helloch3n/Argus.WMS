using Argus.WMS.Inbound;
using Argus.WMS.MasterData;
using Argus.WMS.MasterData.Reels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class ReceiptDetailConfiguration : IEntityTypeConfiguration<ReceiptDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
        {
            builder.ToTable("AppReceiptDetails");

            builder
                .HasOne<Receipt>()
                .WithMany(x => x.Details)
                .HasForeignKey(x => x.ReceiptId);

            builder
                .HasOne(x => x.Reel)
                .WithMany()
                .HasForeignKey(x => x.ReelId);

            builder
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

            builder
                .Property(x => x.Unit)
                .IsRequired();
        }
    }
}