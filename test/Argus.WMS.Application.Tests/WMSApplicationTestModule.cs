using Volo.Abp.Modularity;

namespace Argus.WMS;

[DependsOn(
    typeof(WMSApplicationModule),
    typeof(WMSDomainTestModule)
)]
public class WMSApplicationTestModule : AbpModule
{

}
