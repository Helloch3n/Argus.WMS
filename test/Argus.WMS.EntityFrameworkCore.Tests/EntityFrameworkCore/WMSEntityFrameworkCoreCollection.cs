using Xunit;

namespace Argus.WMS.EntityFrameworkCore;

[CollectionDefinition(WMSTestConsts.CollectionDefinitionName)]
public class WMSEntityFrameworkCoreCollection : ICollectionFixture<WMSEntityFrameworkCoreFixture>
{

}
