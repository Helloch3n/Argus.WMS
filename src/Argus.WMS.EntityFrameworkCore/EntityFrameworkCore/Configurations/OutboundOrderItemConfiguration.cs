using Argus.WMS.Outbound;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class OutboundOrderItemConfiguration : IEntityTypeConfiguration<OutboundOrderItem>
    {
        public void Configure(EntityTypeBuilder<OutboundOrderItem> builder)
        {
            builder.ToTable("AppOutboundOrderItems");

            builder.ConfigureByConvention();

            builder.Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("物料编码");

            builder.Property(x => x.TargetLength)
                .HasPrecision(18, 2)
                .HasComment("单根目标长度");

            builder.Property(x => x.Quantity)
                .HasComment("需求件数");

            builder.Property(x => x.AllocatedQuantity)
                .HasComment("已分配件数");
        }
    }
}