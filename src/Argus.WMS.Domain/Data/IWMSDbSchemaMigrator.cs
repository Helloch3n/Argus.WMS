using System.Threading.Tasks;

namespace Argus.WMS.Data;

public interface IWMSDbSchemaMigrator
{
    Task MigrateAsync();
}
