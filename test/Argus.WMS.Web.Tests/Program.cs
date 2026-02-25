using Microsoft.AspNetCore.Builder;
using Argus.WMS;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Argus.WMS.Web.csproj"); 
await builder.RunAbpModuleAsync<WMSWebTestModule>(applicationName: "Argus.WMS.Web");

public partial class Program
{
}
