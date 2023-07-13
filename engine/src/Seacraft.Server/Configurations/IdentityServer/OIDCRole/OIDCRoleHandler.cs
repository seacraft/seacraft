// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'OIDCRoleHandler.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Seacraft.Server.Configurations.IdentityServer.OIDCRole
{
    public class OIDCRoleHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated && context.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value == null)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
