// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftMemoryCache.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Seacraft.Server.Configurations.IdentityServer.Extentions
{

    public interface ISeacraftCache
    {
        void Set<T>(string key, T value, long expired = -1) where T : class;

        T Get<T>(string key, bool remove = false) where T : class;

        void Remove(string key);
    }

    public class SeacraftMemoryCache : ISeacraftCache
    {
        private readonly IMemoryCache cache;
        private readonly ILogger<SeacraftMemoryCache> logger;

        public SeacraftMemoryCache(IMemoryCache cache, ILogger<SeacraftMemoryCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public void Set<T>(string key, T value, long expired = -1) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                logger.LogError($"缓存key不能为空");
            }

            if (value == null)
            {
                logger.LogError($"缓存key不能为空");
            }
            var random = new Random();

            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(random.Next(1, 5))
            };

            if (expired != -1)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(expired);
            }

            cache.Set(key, value, options);
        }

        public T Get<T>(string key, bool remove = false) where T : class
        {
            var result = cache.Get<T>(key);
            if (remove)
            {
                this.Remove(key);
            }
            return result;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }

    public class SynyiDistributeCache : ISeacraftCache
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<SynyiDistributeCache> logger;

        public SynyiDistributeCache(IDistributedCache cache, ILogger<SynyiDistributeCache> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public void Set<T>(string key, T value, long expired = -1) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                logger.LogError($"缓存key不能为空");
            }

            if (value == null)
            {
                logger.LogError($"缓存key不能为空");
            }

            var json = JsonSerializer.Serialize(value);
            var random = new Random();
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(random.Next(1, 5))
            };


            if (expired != -1)
            {
                options.SlidingExpiration = TimeSpan.FromSeconds(expired);
            }
            cache.SetString(key, json, options);
        }

        public T Get<T>(string key, bool remove = false) where T : class
        {
            var data = cache.GetString(key);

            if (string.IsNullOrEmpty(data)) return default;

            var entity = JsonSerializer.Deserialize<T>(data);

            if (remove)
            {
                this.Remove(key);
            }

            return entity;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }
}
