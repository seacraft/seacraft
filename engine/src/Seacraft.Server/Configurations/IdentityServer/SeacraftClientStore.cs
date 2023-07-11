// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftClientStore.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Secret = IdentityServer4.Models.Secret;

namespace Seacraft.Server.Configurations.IdentityServer
{
    public class SeacraftClientStore : IClientStore
    {

        /// <summary>
        /// The DbContext.
        /// </summary>
        protected readonly IConfigurationDbContext Context;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<SeacraftClientStore> Logger;

        /// <summary>
        /// The cache
        /// </summary>
        private readonly IMemoryCache cache;

        public SeacraftClientStore(IConfigurationDbContext context, ILogger<SeacraftClientStore> logger, IMemoryCache cache)
        {
            Context = context;
            Logger = logger;
            this.cache = cache;
        }

        public async Task<bool> IsActive(string clientId)
        {
            return await Context.Clients.Where(x => x.ClientId == clientId).AnyAsync(x => x.Enabled);
        }
        private async Task<Client> FindClientByIdForPgAsync(string clientId)
        {
           var client =  new Client
            {
                ClientId = "PassPattern",
                // 无交互用户，请使用clientid/secret进行身份验证
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                // 身份验证的秘密
                ClientSecrets =
                    {
                        new Secret("PassPatternSecret".Sha256())
                    },
                // scopes that client has access to 
                // 客户端有权访问的作用域--可以访问资源---必须在这里定义，才能访问--才能体现在token中
                AllowedScopes = {
                        "Client-ApiScope", //必须是这里声明了Api的Scope,在获取Token的时候获取到  
                        IdentityServerConstants.StandardScopes.OpenId,//必须在IdentityResources中声明过的，才能在这里使用
                        IdentityServerConstants.StandardScopes.Profile,//必须在IdentityResources中声明过的，才能在这里使用
                        IdentityServerConstants.StandardScopes.Email,//必须在IdentityResources中声明过的，才能在这里使用
                    }
            };
            return await Task.FromResult(client);
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            //if (this.Context is ConfigurationDbContext pgdb && pgdb.Database.IsNpgsql())
            if (this.Context is ConfigurationDbContext pgdb && true)
            {
                return await this.FindClientByIdForPgAsync(clientId);
            }

            var key = $"client:{clientId}";
            var data = cache.Get<Client>(key);
            if (data == null)
            {
                var client = await Context.Clients
                           .Include(x => x.AllowedGrantTypes)
                           .Include(x => x.RedirectUris)
                           .Include(x => x.PostLogoutRedirectUris)
                           .Include(x => x.AllowedScopes)
                           .Include(x => x.ClientSecrets)
                           .Include(x => x.Claims)
                           .Include(x => x.IdentityProviderRestrictions)
                           .Include(x => x.AllowedCorsOrigins)
                           .Include(x => x.Properties)
                           .AsNoTracking()
                           .FirstOrDefaultAsync(x => x.ClientId == clientId);
                var model = client?.ToModel();
                Logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, model != null);
                cache.Set(key, model);

                data = model;
            }

            return data;
        }
    }
}
