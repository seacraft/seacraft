// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'RoutingConfig.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server.Configurations
{
    public static class RoutingConfig
    {
        public static IServiceCollection AddApiRoutingConfiguration(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            return services;
        }
    }
}
