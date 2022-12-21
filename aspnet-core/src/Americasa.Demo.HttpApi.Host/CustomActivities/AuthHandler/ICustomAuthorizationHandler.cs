using Americasa.Demo.CustomActivities.Models;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.AuthHandler
{
    public interface ICustomAuthorizationHandler
    {
        ValueTask<bool> AuthorizeAsync(AuthorizeCustomContext context);
    }
}
