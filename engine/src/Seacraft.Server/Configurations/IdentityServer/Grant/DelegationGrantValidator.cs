// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'DelegationGrantValidator.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace Seacraft.Server.Configurations.IdentityServer.Grant
{
    public class DelegationGrantValidator: IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;

        public DelegationGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
        }

        public string GrantType => "delegation";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userToken = context.Request.Raw.Get("token");

            if (string.IsNullOrEmpty(userToken))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorDescription: "No User Token");
                return;
            }

            var result = await _validator.ValidateAccessTokenAsync(userToken);
            if (result.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, errorDescription: "User Token Invaild");
                return;
            }

            // get user's identity
            // TODO: Session?
            var sub = result.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var clientid = result.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "unknown";
            context.Result = new GrantValidationResult(sub, GrantType, claims: new List<Claim> { new Claim("source_client_id", clientid) });
            return;
        }
    }
}
