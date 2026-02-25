using Argus.WMS.Samples;
using Xunit;

namespace Argus.WMS.EntityFrameworkCore.Domains;

[Collection(WMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WMSEntityFrameworkCoreTestModule>
{

}
