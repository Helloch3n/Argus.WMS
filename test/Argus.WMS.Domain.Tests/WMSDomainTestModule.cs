using Volo.Abp.Modularity;

namespace Argus.WMS;

[DependsOn(
    typeof(WMSDomainModule),
    typeof(WMSTestBaseModule)
)]
public class WMSDomainTestModule : AbpModule
{

}
