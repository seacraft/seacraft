// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ApplicationBuilderExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ShowCommonExtensions();
                options.EnableFilter();
                options.DisplayRequestDuration();
            });
            return app;
        }
    }
}
