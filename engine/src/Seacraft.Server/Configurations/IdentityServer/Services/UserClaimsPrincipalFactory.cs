// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'UserClaimsPrincipalFactory.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Seacraft.Server.Configurations.IdentityServer.Domains;
using System.Security.Claims;

namespace Seacraft.Server.Configurations.IdentityServer.Services
{
    public class UserClaimsPrincipalFactory<TUser> : Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory<TUser>
      where TUser : ApplicationUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserClaimsPrincipalFactory(
            UserManager<TUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            IHttpContextAccessor httpContextAccessor
            ) : base(userManager, optionsAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var id = new ClaimsIdentity("Identity.Application", // REVIEW: Used to match Application scheme
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, user.UserName));
            id.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));
            id.AddClaim(new Claim(JwtClaimTypes.NickName, user.Name));
            var context = httpContextAccessor.HttpContext;
            if (context != null)
            {
                id.AddClaim(new Claim("ip", context.Connection.RemoteIpAddress.ToString()));
            }
            if (UserManager.SupportsUserEmail)
            {
                var email = user.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    id.AddClaim(new Claim(JwtClaimTypes.Email, email));
                }
            }
            if (UserManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
                    user.SecurityStamp));
            }
            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }
            if (UserManager.SupportsUserRole)
            {
                id.AddClaims((await UserManager.GetRolesAsync(user)).Select(x => new Claim(JwtClaimTypes.Role, x)));
            }
            return id;
        }
    }
}
