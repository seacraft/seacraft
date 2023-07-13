// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'Program.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Seacraft.Server.Configurations;
using Seacraft.Server.Configurations.IdentityServer.Domains;
using Seacraft.Server.Configurations.IdentityServer.EndPoints;
using Seacraft.Server.Extensions;
using System.Reflection.Metadata;
using Seacraft.Server.Configurations.IdentityServer.Services;
using Seacraft.Server.Configurations.IdentityServer.Validator;
using Seacraft.Server.Configurations.IdentityServer.ResponseHandling;
using Seacraft.Server.Configurations.IdentityServer.Grant;
using Seacraft.Server.Configurations.MessageStore;
using Seacraft.Server.Configurations.IdentityServer.Services.Default;
using Seacraft.Server.Configurations.IdentityServer.OIDCRole;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .AddYamlFile("appsettings.yml", optional: true, reloadOnChange: true)
    .AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yml", true);


// Add services to the container.

builder.Services.AddOptions().AddHttpContextAccessor().AddHttpClient();
builder.Services.AddApiRoutingConfiguration().AddControllers(optipns =>
{
    optipns.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration(builder.Configuration);

// 配置第三方鉴权
var authenticationBuilder = builder.Services.AddAuthentication();

string schemaName = "public";

var  identityServerbuilder = builder.Services.AddIdentityServer(options => 
{
    options.UserInteraction.LoginUrl = "/account/login";
    options.UserInteraction.LogoutUrl = "/account/logout";
    options.UserInteraction.ErrorUrl = "/home/error";
    options.Discovery.CustomEntries.Add("change_password_endpoint", $"~/connect/changepassword");
    options.Discovery.CustomEntries.Add("session_time", 600);
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Discovery.ShowClaims = false;
    options.Authentication.CheckSessionCookieSameSiteMode = SameSiteMode.Unspecified;
    options.Authentication.CheckSessionCookieName = "sso.oidc.session";
}).
AddAspNetIdentity<ApplicationUser>().
AddProfileService<SeacraftProfileService<ApplicationUser>>().
AddEndpoint<ChangePasswordEndPoint>("ChangePassword", "/connect/changepassword");

//添加数据库
identityServerbuilder.AddConfigurationStore(options => 
{
    options.DefaultSchema = schemaName;
    options.ConfigureDbContext = builders =>
    {

    };
});

// IdentityServer4会修改这两个Cookie的SameSite，所以要放在这里修改
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "sso.cookie";
    // 这样会导致Iframe无法获取Cookie
    // 不过如果是同一个域不会有这个问题
    // 例如a.Seacreaft.com 和 b.Seacreaft.com 会被视作是same-site
    // TODO: 这里要做成一个配置项
    options.Cookie.SameSite = SameSiteMode.Unspecified;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromHours(12);
});
builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Unspecified;
    options.Cookie.Name = "sso.external";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var services = builder.Services;
// 支持生成和验证离线模式的特殊JWT Token
services.AddTransient<ITokenService, SeacraftTokenService>();
services.AddTransient<ITokenValidator, SeacraftTokenValidator>();
// Introspect 接口支持Superior属性的ApiResource，可以获取所有token的信息
services.AddTransient<IIntrospectionResponseGenerator, MyIntrospectionResponseGenerator>();
//// 支持Delegation方式获取Token
services.AddTransient<IExtensionGrantValidator, DelegationGrantValidator>();
services.AddTransient<IEventSink, LoggingEventSink>();
//// 使用自定义principle生成器
services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, Seacraft.Server.Configurations.IdentityServer.Services.UserClaimsPrincipalFactory<ApplicationUser>>();
services.AddTransient<IdentityServerTools>();
//// 配置随机密码生成器
services.AddTransient<IPasswordGenerator, DefaultPasswordGenerator>();

//// 配置LogoutID Store，使用分布式缓存
services.AddTransient<IMessageStore<LogoutMessage>, SeacraftLogoutMessageStore>();

//// 配置RBAC鉴权&授权
services.AddAuthentication().AddLocalApi(options =>
{
    options.ExpectedScope = "Seacraftiam";
});
services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
    {
        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        //policy.AddRequirements();
        policy.RequireRole("iam-admin");
    });
});

services.AddSingleton<IAuthorizationHandler, OIDCRoleHandler>();

//// TODO: Remove This After .Net Core 6.0
services.AddTransient<CookieAuthenticationHandler, MyCookieAuthenticationHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();

app.Run();
