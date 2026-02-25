using Argus.WMS.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("AppProducts");

            builder.ConfigureByConvention();

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("物料编码");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("物料名称");


            builder.Property(x => x.Unit)
                .HasMaxLength(20)
                .HasComment("单位");

            builder.Property(x => x.Length)
                .HasPrecision(18, 2)
                .HasComment("长度(cm)");

            builder.Property(x => x.Width)
                .HasPrecision(18, 2)
                .HasComment("宽度(cm)");

            builder.Property(x => x.Height)
                .HasPrecision(18, 2)
                .HasComment("高度(cm)");

            builder.Property(x => x.Weight)
                .HasPrecision(18, 2)
                .HasComment("重量(kg)");

            builder.Property(x => x.IsBatchManagementEnabled)
                .HasComment("批次管理");

            builder.Property(x => x.ShelfLifeDays)
                .HasComment("保质期(天)");

            builder.Ignore(x => x.Volume);

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}