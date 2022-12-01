using Americasa.Demo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Americasa.Demo;

[DependsOn(
    typeof(DemoEntityFrameworkCoreTestModule)
    )]
public class DemoDomainTestModule : AbpModule
{

}
