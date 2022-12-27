using Americasa.Demo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Americasa.Demo.Permissions;

public class HouseDealsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup("Americasa");
        //Define your own permissions here. Example:
        myGroup.AddPermission("Americasa_Deals_Create");
        myGroup.AddPermission("Americasa_Deals_View");
        myGroup.AddPermission("Americasa_Deals_StartHomeReview"); 
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DemoResource>(name);
    }
}
