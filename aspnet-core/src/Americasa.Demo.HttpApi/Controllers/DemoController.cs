using Americasa.Demo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Americasa.Demo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DemoController : AbpControllerBase
{
    protected DemoController()
    {
        LocalizationResource = typeof(DemoResource);
    }
}
