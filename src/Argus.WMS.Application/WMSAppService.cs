using Argus.WMS.Localization;
using Volo.Abp.Application.Services;

namespace Argus.WMS;

/* Inherit your application services from this class.
 */
public abstract class WMSAppService : ApplicationService
{
    protected WMSAppService()
    {
        LocalizationResource = typeof(WMSResource);
    }
}
