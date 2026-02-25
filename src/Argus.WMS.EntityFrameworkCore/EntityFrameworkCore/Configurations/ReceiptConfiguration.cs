using Argus.WMS.Inbound;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.ToTable("AppReceipts");

            builder.HasIndex(x => x.BillNo).IsUnique();
        }
    }
}