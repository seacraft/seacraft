// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'AuthorizationConfig.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Seacraft.Server.Configurations
{
    public static class AuthorizationConfig
    {

        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources()
                .AddInMemoryApiScopes()
                .AddInMemoryClients()
                .AddInMemoryApiResources()
                .AddTestUsers()
                .AddDeveloperSigningCredential();
            return services;
        }

        private static IIdentityServerBuilder AddInMemoryIdentityResources(this IIdentityServerBuilder builder)
        {
            var list = new IdentityResource[]
          {
                   new IdentityResources.OpenId(),
                   new IdentityResources.Profile(),
                   //new IdentityResources.Email(),//内置的信息
                   new IdentityResources.Email{
                        Enabled=true,//是否启用，默认为true
                        DisplayName="这里是修改过的DisplayName", //显示的名称，如在同意界面中将使用此值
                        Name="这里是修改过的-身份资源的唯一名称--Name",
                        Description="这里是修改过的Description", //显示的描述，如在同意界面中将使用此值。
                        Required=true,//指定用户是否可以在同意界面中取消选择范围（如果同意界面要实现这样的功能）。false 表示可以取消，true 则为必须。默认为 false。 
                   },//内置的信息
                   new IdentityResources.Phone(),//内置的信息
                   new IdentityResources.Address(),
                   new IdentityResource("roles", "角色信息", new List<string> {JwtClaimTypes.Role}) //自定义的信息
          };

            return builder.AddInMemoryIdentityResources(list);
        }

        private static IIdentityServerBuilder AddInMemoryApiScopes(this IIdentityServerBuilder builder)
        {
            var list = new ApiScope[]
           {
                new ApiScope()
                {
                 Name="Client-ApiScope"
                }
           };
            return builder.AddInMemoryApiScopes(list);
        }

        private static IIdentityServerBuilder AddInMemoryClients(this IIdentityServerBuilder builder)
        {
            var list = new Client[]
            {
                  new Client
                    {
                        ClientId = "ClientPattern", 
                        // 无交互用户，请使用clientid/secret进行身份验证
                        AllowedGrantTypes = GrantTypes.ClientCredentials, 
                        // 身份验证的秘钥
                        ClientSecrets =
                        {
                            new Secret("ClientPatternSecret".Sha256())
                        }, 
                        // scopes that client has access to 
                        // 客户端有权访问的作用域--可以访问资源---必须在这里定义，才能访问--才能体现在token中
                        AllowedScopes = {
                            "Client-ApiScope" //必须是这里声明了Api的Scope,在获取Token的时候获取到  
                        }
                    },
                 new Client
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
                }
            };

            return builder.AddInMemoryClients(list);
        }

        private static IIdentityServerBuilder AddInMemoryApiResources(this IIdentityServerBuilder builder)
        {
            var list = new ApiResource[]{
                new ApiResource()
                {
                 Name="test",
                 DisplayName="test 显示",
                Scopes=new  List<string>{
                        "Client-ApiScope"
                    }
                }
            };
            return builder.AddInMemoryApiResources(list);
        }
        private static IIdentityServerBuilder AddTestUsers(this IIdentityServerBuilder builder)
        {
            var list = new List<TestUser>
            {
                new TestUser{

                    SubjectId = "1",
                    //ProviderSubjectId="pwdClient",
                    Username = "huhouhua",
                    Password = "huhouhua",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "huhouhua"),
                        new Claim(JwtClaimTypes.GivenName, "huhouhua"),
                        new Claim(JwtClaimTypes.FamilyName, "huhouhua"),
                        new Claim(JwtClaimTypes.Email, "huhouhua@email.com"),
                    }
                }
            };
            return builder.AddTestUsers(list);
        }
    }
}
