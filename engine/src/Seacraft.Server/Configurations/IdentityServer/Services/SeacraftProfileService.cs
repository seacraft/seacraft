// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftProfileService.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Seacraft.Server.Configurations.IdentityServer.Domains;
using Seacraft.Server.Configurations.IdentityServer.Extentions.ExternalUser.Services;
using System.Security.Claims;

namespace Seacraft.Server.Configurations.IdentityServer.Services
{

    /// <summary>
    /// IProfileService to integrate with ASP.NET Identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class SeacraftProfileService<TUser> : IProfileService
        where TUser : class
    {
        private static HttpClient HttpClient = new HttpClient();
        /// <summary>
        /// The claims factory.
        /// </summary>
        protected readonly IUserClaimsPrincipalFactory<TUser> ClaimsFactory;
        private readonly IEnumerable<IExternalUserClaimProvider> externalUserClaims;
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger<SeacraftProfileService<TUser>> Logger;

        /// <summary>
        /// The user manager.
        /// </summary>
        protected readonly UserManager<TUser> UserManager;
        private HttpContext HttpContext;
        private readonly IResourceStore ResourceStore;
        private readonly ISystemClock Clock;
        private readonly IReferenceTokenStore ReferenceTokenStore;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        public SeacraftProfileService(UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IHttpContextAccessor httpContextAccessor,
            IResourceStore resourceStore,
            ISystemClock clock,
            IReferenceTokenStore referenceTokenStore,
            IServiceProvider serviceProvider,
            IEnumerable<IExternalUserClaimProvider> externalUserClaims
            )
        {
            UserManager = userManager;
            ClaimsFactory = claimsFactory;
            HttpContext = httpContextAccessor.HttpContext;
            ResourceStore = resourceStore;
            ReferenceTokenStore = referenceTokenStore;
            this.Clock = clock;
            this.serviceProvider = serviceProvider;
            this.externalUserClaims = externalUserClaims;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="claimsFactory">The claims factory.</param>
        /// <param name="logger">The logger.</param>
        public SeacraftProfileService(UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            ILogger<SeacraftProfileService<TUser>> logger,
            IHttpContextAccessor httpContextAccessor,
            IResourceStore resourceStore,
            ISystemClock clock,
            IReferenceTokenStore referenceTokenStore,
            IServiceProvider serviceProvider,
            IEnumerable<IExternalUserClaimProvider> externalUserClaims
            ) : this(userManager, claimsFactory, httpContextAccessor, resourceStore, clock, referenceTokenStore, serviceProvider, externalUserClaims)
        {
            Logger = logger;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new ArgumentException("No sub claim present");

            await GetProfileDataAsync(context, sub);
        }

        /// <summary>
        /// Called to get the claims for the subject based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await GetProfileDataAsync(context, user);
            }
        }

        /// <summary>
        /// Called to get the claims for the user based on the profile request.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, TUser user)
        {
            var claims = (await GetUserClaimsAsync(user)).Claims.ToList();
            // 需要把apiResource所需要的Claims也聚合在Userinfo里
            var apiClaims = (await ResourceStore.FindApiResourcesByScopeNameAsync(context.Client.AllowedScopes)).Select(x => x.UserClaims).SelectMany(i => i);
            context.RequestedClaimTypes = context.RequestedClaimTypes.ToList();
            (context.RequestedClaimTypes as List<string>).AddRange(apiClaims);
            (context.RequestedClaimTypes as List<string>).Add("ip");
            // 读取插件提供的外部Claims
            foreach (var externalClaimProvider in this.externalUserClaims)
            {
                var supportedClaims = await externalClaimProvider.GetSupportClaims();
                if (supportedClaims != null && context.RequestedClaimTypes.Any(x => supportedClaims.Contains(x)))
                {
                    var externalClaims = await externalClaimProvider.GetUserClaims(claims);
                    claims.AddRange(externalClaims);
                }
            }
            context.AddRequestedClaims(claims);
        }

        /// <summary>
        /// Gets the claims for a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task<ClaimsPrincipal> GetUserClaimsAsync(TUser user)
        {
            var principal = await ClaimsFactory.CreateAsync(user);
            if (principal == null) throw new ArgumentException("ClaimsFactory failed to create a principal");

            return principal;
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null) throw new ArgumentException("No subject Id claim present");

            await IsActiveAsync(context, sub);
        }

        /// <summary>
        /// Determines if the subject is active.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task IsActiveAsync(IsActiveContext context, string subjectId)
        {
            var user = await FindUserAsync(subjectId);
            if (user != null)
            {
                await IsActiveAsync(context, user);
            }
            else
            {
                context.IsActive = false;
            }
        }

        /// <summary>
        /// Determines if the user is active.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual async Task IsActiveAsync(IsActiveContext context, TUser user)
        {
            context.IsActive = await IsUserActiveAsync(user);
        }

        /// <summary>
        /// Returns if the user is active.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<bool> IsUserActiveAsync(TUser user)
        {
            if (user is ApplicationUser appUser)
            {
                if (appUser.Status != ActivityStatus.Active)
                {
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Loads the user by the subject id.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        protected virtual async Task<TUser> FindUserAsync(string subjectId)
        {
            var user = await UserManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                Logger?.LogWarning("No user found matching subject Id: {subjectId}", subjectId);
            }

            return user;
        }
    }
}
