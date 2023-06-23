// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SwaggerConfig.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Seacraft.Server.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Server",
                    Version = "1",
                    Description = "Seacraft Server "
                });
                options.OrderActionsBy(o => o.RelativePath);
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file, true);
                });
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
                //权限Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
