using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RealEstateApp.Data.Models;

namespace RealEstateApp.Web.Areas.Identity.Infrastructure
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        public CustomSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation
            ) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByEmailAsync(userName);

            if (user == null || user.IsDeleted == true)
            {
                return SignInResult.Failed;
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        }

        public override async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            if (user == null || user.IsDeleted == true)
            {
                return SignInResult.Failed;
            }

            return await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

        }
    }
}
