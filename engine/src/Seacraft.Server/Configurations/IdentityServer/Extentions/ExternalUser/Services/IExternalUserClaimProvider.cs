// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IExternalUserClaimProvider.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System.Security.Claims;

namespace Seacraft.Server.Configurations.IdentityServer.Extentions.ExternalUser.Services
{
    public interface IExternalUserClaimProvider
    {
        Task<IEnumerable<string>> GetSupportClaims();
        Task<IEnumerable<Claim>> GetUserClaims(IEnumerable<Claim> claims);
    }
}
