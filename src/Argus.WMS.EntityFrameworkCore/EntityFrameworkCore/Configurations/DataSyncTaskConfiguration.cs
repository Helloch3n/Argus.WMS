using Argus.WMS.DataSync;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Argus.WMS.EntityFrameworkCore.Configurations
{
    public class DataSyncTaskConfiguration : IEntityTypeConfiguration<DataSyncTask>
    {
        public void Configure(EntityTypeBuilder<DataSyncTask> builder)
        {
            builder.ToTable("WmsDataSyncTasks");

            builder.ConfigureByConvention();

            builder.Property(x => x.TaskCode)
                .IsRequired()
                .HasMaxLength(64)
                .HasComment("任务编码");

            builder.HasIndex(x => x.TaskCode)
                .IsUnique();

            builder.Property(x => x.TaskName)
                .IsRequired()
                .HasMaxLength(128)
                .HasComment("任务名称");

            builder.Property(x => x.CronExpression)
                .IsRequired()
                .HasMaxLength(32)
                .HasComment("Cron 表达式");

            builder.Property(x => x.LastSyncMessage)
                .HasMaxLength(1024)
                .HasComment("上次执行日志或异常摘要");
        }
    }
}
