using Argus.WMS.Samples;
using Xunit;

namespace Argus.WMS.EntityFrameworkCore.Applications;

[Collection(WMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WMSEntityFrameworkCoreTestModule>
{

}
