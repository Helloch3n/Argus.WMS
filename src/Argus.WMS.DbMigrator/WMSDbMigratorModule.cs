using Argus.WMS.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Argus.WMS.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WMSEntityFrameworkCoreModule),
    typeof(WMSApplicationContractsModule)
)]
public class WMSDbMigratorModule : AbpModule
{
}
