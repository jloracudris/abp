using Americasa.Demo.CustomActivities.Models;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Americasa.Demo.CustomActivities.AuthHandler
{
    public interface ICustomAuthorizationHandler
    {
        ValueTask<bool> AuthorizeAsync(AuthorizeCustomContext authorizeCustomContext);
    }
}
