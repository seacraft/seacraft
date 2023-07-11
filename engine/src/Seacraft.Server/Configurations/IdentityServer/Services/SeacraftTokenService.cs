// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftTokenService.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Security.Claims;
using Seacraft.Server.Configurations.IdentityServer.Options;
using Microsoft.AspNetCore.Authentication;
using Seacraft.Server.Configurations.IdentityServer.Extentions;
using IdentityServer4.Extensions;
using Seacraft.Server.Configurations.IdentityServer.Constant;

namespace Seacraft.Server.Configurations.IdentityServer.Services
{
    public class SeacraftTokenService : DefaultTokenService
    {
        private readonly SeacraftIdentityOptions options;
        public SeacraftTokenService(
            IClaimsService claimsProvider,
            IReferenceTokenStore referenceTokenStore,
            ITokenCreationService creationService,
            IHttpContextAccessor contextAccessor,
            ISystemClock clock,
            IKeyMaterialService keyMaterialService,
            IdentityServerOptions options,
            IOptions<SeacraftIdentityOptions> seaOptions,
            ILogger<DefaultTokenService> logger) : base(claimsProvider, referenceTokenStore, creationService, contextAccessor, clock, keyMaterialService, options, logger)
        {
            this.options = seaOptions.Value;
        }

        public override async Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            // 离线模式下要在IdentityToken里面加上全套用户信息
            if (options.EnableOfflineMode)
            {
                request.IncludeAllIdentityClaims = true;
            }
            var result = await base.CreateIdentityTokenAsync(request);
            return result;
        }

        public override async Task<string> CreateSecurityTokenAsync(Token token)
        {
            var t = await base.CreateSecurityTokenAsync(token);
            if (options.EnableOfflineMode && token.Type == OidcConstants.TokenTypes.AccessToken && token.AccessTokenType == AccessTokenType.Reference)
            {
                // create一个Jwt出来，然后把t作为claim-Reference 塞进去
                token.Claims.Add(new Claim(Claims.ReferenceHash, t));
                // TODO: 离线模式Token有效期
                //token.Lifetime = 
                if (token.AllowedSigningAlgorithms == null || token.AllowedSigningAlgorithms.Count == 0)
                {
                    token.AllowedSigningAlgorithms = new List<string> { "ES256" };
                }
                var jwtResult = await CreationService.CreateTokenAsync(token);
                return jwtResult;
            }
            return t;
        }

        public override async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            Logger.LogTrace("Creating access token");
            request.Validate();

            var claims = new List<Claim>();
            claims.AddRange(await ClaimsProvider.GetAccessTokenClaimsAsync(
                request.Subject,
                request.ValidatedResources,
                request.ValidatedRequest));

            // 添加name和nickname

            var name = request.Subject?.Claims?.FirstOrDefault(x => x.Type == JwtClaimTypes.Name);
            if (name != null)
            {
                claims.Add(name);
            }
            var nickname = request.Subject?.Claims?.FirstOrDefault(x => x.Type == JwtClaimTypes.NickName);
            if (nickname != null)
            {
                claims.Add(nickname);
            }
            var source_client_id = request.Subject?.Claims?.FirstOrDefault(x => x.Type == "source_client_id");
            if (source_client_id != null)
            {
                claims.Add(source_client_id);
            }

            if (request.ValidatedRequest.Client.IncludeJwtId)
            {
                claims.Add(new Claim(JwtClaimTypes.JwtId, CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)));
            }

            if (request.ValidatedRequest.SessionId.IsPresent())
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, request.ValidatedRequest.SessionId));
            }
            // iat claim as required by JWT profile
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, Clock.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64));

            var issuer = ContextAccessor.HttpContext.GetIdentityServerIssuerUri();
            var token = new Token(OidcConstants.TokenTypes.AccessToken)
            {
                CreationTime = Clock.UtcNow.UtcDateTime,
                Issuer = issuer,
                Lifetime = request.ValidatedRequest.AccessTokenLifetime,
                Claims = claims.Distinct(new ClaimComparer()).ToList(),
                ClientId = request.ValidatedRequest.Client.ClientId,
                Description = request.Description,
                AccessTokenType = request.ValidatedRequest.AccessTokenType,
                AllowedSigningAlgorithms = request.ValidatedResources.Resources.ApiResources.FindMatchingSigningAlgorithms()
            };

            // add aud based on ApiResources in the validated request
            foreach (var aud in request.ValidatedResources.Resources.ApiResources.Select(x => x.Name).Distinct())
            {
                token.Audiences.Add(aud);
            }

            if (Options.EmitStaticAudienceClaim)
            {
                token.Audiences.Add(string.Format(IdentityServerConstants.AccessTokenAudience, issuer.EnsureTrailingSlash()));
            }

            // add cnf if present
            if (request.ValidatedRequest.Confirmation.IsPresent())
            {
                token.Confirmation = request.ValidatedRequest.Confirmation;
            }
            else
            {
                if (Options.MutualTls.AlwaysEmitConfirmationClaim)
                {
                    var clientCertificate = await ContextAccessor.HttpContext.Connection.GetClientCertificateAsync();
                    if (clientCertificate != null)
                    {
                        token.Confirmation = clientCertificate.CreateThumbprintCnf();
                    }
                }
            }

            return token;
        }
    }
}
