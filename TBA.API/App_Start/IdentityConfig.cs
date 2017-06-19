using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Security.Claims;
using TBA.Model;
using System.Security.Principal;
using TBA.API.App_Start;

namespace TBA.API
{
    public class ApplicationUserManager : UserManager<AppUser, int>
    {
        public ApplicationUserManager() : base(new UserStore())
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager();
            manager.UserLockoutEnabledByDefault = false;
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser, Int32>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public static class IdentityExtensions
    {

        public static string GetUserDeviceId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("device_id");
            return (claim != null) ? claim.Value : "";
        }

        public static string GetUserDeviceType(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("device_type");
            return (claim != null) ? claim.Value : "";
        }
    }
}