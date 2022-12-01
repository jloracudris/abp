using System.Threading.Tasks;

namespace Americasa.Demo.Data;

public interface IDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
