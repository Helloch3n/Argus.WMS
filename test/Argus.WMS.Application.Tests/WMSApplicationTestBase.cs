using Volo.Abp.Modularity;

namespace Argus.WMS;

public abstract class WMSApplicationTestBase<TStartupModule> : WMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
