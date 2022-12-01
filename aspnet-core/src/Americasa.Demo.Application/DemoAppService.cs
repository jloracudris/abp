using System;
using System.Collections.Generic;
using System.Text;
using Americasa.Demo.Localization;
using Volo.Abp.Application.Services;

namespace Americasa.Demo;

/* Inherit your application services from this class.
 */
public abstract class DemoAppService : ApplicationService
{
    protected DemoAppService()
    {
        LocalizationResource = typeof(DemoResource);
    }
}
