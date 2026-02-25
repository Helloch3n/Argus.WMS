using Argus.WMS.Outbound;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class OutboundOrderConfiguration : IEntityTypeConfiguration<OutboundOrder>
    {
        public void Configure(EntityTypeBuilder<OutboundOrder> builder)
        {
            builder.ToTable("AppOutboundOrders");

            builder.ConfigureByConvention();

            builder.Property(x => x.OrderNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("出库单号");

            builder.Property(x => x.SourceOrderNo)
                .HasMaxLength(50)
                .HasComment("外部单号");

            builder.Property(x => x.CustomerName)
                .HasMaxLength(200)
                .HasComment("客户名称");

            builder.Property(x => x.Status)
                .HasComment("状态");

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.OutboundOrderId)
                .IsRequired();
        }
    }
}