using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brewery.BusinessLogic.Service
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        private readonly UserManager<IdentityUser> userManager;

        public AppClaimsPrincipalFactory(
                UserManager<IdentityUser> userManager
                , RoleManager<IdentityRole> roleManager
                , IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, roleManager, optionsAccessor)
        {
            this.userManager = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);

                ((ClaimsIdentity)principal.Identity).AddClaim(roleClaim);
            }

            return principal;
        }
    }
}
