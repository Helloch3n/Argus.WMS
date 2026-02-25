using Argus.WMS.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Argus.WMS.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WMSController : AbpControllerBase
{
    protected WMSController()
    {
        LocalizationResource = typeof(WMSResource);
    }
}
