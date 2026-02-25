using Volo.Abp.Modularity;

namespace Argus.WMS;

/* Inherit from this class for your domain layer tests. */
public abstract class WMSDomainTestBase<TStartupModule> : WMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
