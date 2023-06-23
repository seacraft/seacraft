﻿// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IdServiceExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Medallion.Threading;
using Medallion.Threading.MySql;
using Medallion.Threading.Postgres;
using Medallion.Threading.Redis;
using Medallion.Threading.SqlServer;
using Medallion.Threading.WaitHandles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Seacraft.Services;
using Snowflake;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class IdServiceExtensions
    {

        public static IServiceCollection AddDistributedLock<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var dbContext = sp.GetRequiredService<TContext>();
                var connection = dbContext.Database.GetDbConnection();
                switch (dbContext.Database)
                {
                    case DatabaseFacade db when db.IsMySql():
                        return new MySqlDistributedSynchronizationProvider(connection);

                    case DatabaseFacade db when db.IsSqlServer():
                        return new SqlDistributedSynchronizationProvider(connection);

                    case DatabaseFacade db when db.IsNpgsql():
                        return new PostgresDistributedSynchronizationProvider(connection);

                    default:
                        var redisOptions = sp.GetService<IOptions<RedisCacheOptions>>();
                        if (redisOptions?.Value != null)
                        {
                            var redisConnection = ConnectionMultiplexer.Connect(redisOptions.Value.Configuration);
                            return new RedisDistributedSynchronizationProvider(redisConnection.GetDatabase());
                        }
                        return new WaitHandleDistributedSynchronizationProvider();
                }
            });

            return services;
        }

        public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, Action<RedisCacheOptions> setup)
        {
            services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var redisOptions = sp.GetService<IOptions<RedisCacheOptions>>();
                var options = redisOptions?.Value;
                if (options == null)
                {
                    options = new RedisCacheOptions();
                    setup?.Invoke(options);
                }

                var redisConnection = ConnectionMultiplexer.Connect(options.Configuration);
                return new RedisDistributedSynchronizationProvider(redisConnection.GetDatabase());
            });

            return services;
        }

        public static IServiceCollection AddMemoryDistributedLock(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedLockProvider>(_ =>
            {
                return new WaitHandleDistributedSynchronizationProvider();
            });

            return services;
        }

        public static IServiceCollection AddSnowFlakeService(this IServiceCollection services, Action<SnowFlakeOptions> setup)
        {
            services.Configure(setup);
            services.AddSingleton<SnowFlakeService>();
            services.AddHostedService(provider =>
                provider.GetRequiredService<SnowFlakeService>());
            services.AddSingleton(sp =>
            {
                var snowFlakeService = sp.GetRequiredService<SnowFlakeService>();
                return snowFlakeService.CreateSnowFlakeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            });
            services.AddTransient<IIdService, IdService>();
            return services;
        }

    }

}