using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Argus.WMS.Data;

/* This is used if database provider does't define
 * IWMSDbSchemaMigrator implementation.
 */
public class NullWMSDbSchemaMigrator : IWMSDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
