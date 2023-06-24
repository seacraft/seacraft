using Medallion.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seacraft.Abstractions.Hosting;
using Snowflake;
using System.Net;
using System.Runtime.CompilerServices;

namespace Seacraft.SnowFlake.Distributed
{
    public class SnowFlakeService
            : PeriodicHostedService, IAsyncDisposable
    {

        public const ulong KeyTimeout = 60 * 60 * 24;
        public const ulong PeriodInterval = 1000 * 60 * 60 * 23;
        public const int IndexStart = 0;
        public const int IndexEnd = 64;

        private readonly IDistributedCache _cache;
        private readonly IDistributedLockProvider _distributedLockProvider;

        protected static string LocalIp { get; set; } = GetIpAddress();

        protected static string CacheLockKey { get; } = $"{typeof(SnowFlakeService).FullName}";

        public long DatacenterId { get; }

        public string Name { get; }

        public long MachineId { get; set; }

        public override TimeSpan Period => TimeSpan.FromSeconds(PeriodInterval);

        public string Key
        {
            get
            {
                return $"{Name}:{DatacenterId}:{MachineId}";
            }
        }

        protected string NameAndDataCenterId
        {
            get
            {
                return $"{Name}:{DatacenterId}";
            }
        }

        public SnowFlakeService(
            IDistributedCache cache,
            IDistributedLockProvider distributedLock,
            IServiceScopeFactory factory,
            ILogger<SnowFlakeService> logger,
            IOptions<SnowFlakeOptions> options)
            : base(logger, factory)
        {
            _cache = cache;
            _distributedLockProvider = distributedLock;

            Name = options.Value.Name;
            DatacenterId = options.Value.DataCenterId;
        }

        protected static string GetIpAddress()
        {
            var addressIp = string.Empty;
            var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var ipAddress in addressList)
            {
                var addressFamily = ipAddress.AddressFamily.ToString();
                if (string.Equals(addressFamily, "InterNetwork", StringComparison.InvariantCultureIgnoreCase))
                {
                    return ipAddress.ToString();
                }
            }

            return addressIp;
        }

        public async Task<Snowflake.SnowFlake> CreateSnowFlakeAsync()
        {
            var @lock = _distributedLockProvider.CreateLock(CacheLockKey);
            await using (await @lock.AcquireAsync())
            {
                var ip = long.Parse(LocalIp.Replace(".", "")); //1921680200
                MachineId = ip % 32; //0-31

                var machineId = await CreateMachineIdAsync(LocalIp, MachineId);
                if (machineId != null)
                {
                    MachineId = machineId.Value;
                }

                return new Snowflake.SnowFlake(DatacenterId, MachineId);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string MakeKey(long index)
        {
            return $"{Name}:{DatacenterId}:{index}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual long GetRandomMachineId(long begin, long end)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            return random.NextInt64(begin, end);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task<bool> CheckIndexAvailableAsync(long index, string localIp)
        {
            var key = MakeKey(index);
            var item = await _cache.GetStringAsync(key);
            return string.IsNullOrEmpty(item) ||
                string.Equals(item, localIp, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<long?> GetAvailableMachineIdAsync(long begin, long end, string localIp)
        {
            for (var i = begin; i < end; i++)
            {
                if (await CheckIndexAvailableAsync(i, localIp))
                {
                    return i;
                }
            }

            return null;
        }

        private async Task<bool> RegisterMachineAsync(long machineId, string localIp)
        {
            try
            {
                var key = MakeKey(machineId);
                var val = await _cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(val))
                {
                    await _cache.SetStringAsync(key, localIp, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(KeyTimeout)
                    });
                    return true;
                }
                else
                {
                    return localIp.Equals(val, StringComparison.InvariantCultureIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }

        protected async Task<long?> CreateMachineIdAsync(string localIp, long? machineId = null, CancellationToken? token = null)
        {
            while (!(token?.IsCancellationRequested ?? false))
            {
                if (machineId != null)
                {
                    if (await CheckIndexAvailableAsync(machineId.Value, localIp))
                    {
                        return machineId;
                    }
                }

                machineId = GetRandomMachineId(IndexStart, IndexEnd);
                if (await RegisterMachineAsync(machineId.Value, localIp))
                {
                    return machineId;
                }

                var availableId = await GetAvailableMachineIdAsync(IndexStart, IndexEnd, LocalIp);
                if (availableId != null)
                {
                    return availableId.Value;
                }

                await Task.Delay(100);
            }

            return null;
        }

        protected override async Task<bool> ExecuteWithinAsync(AsyncServiceScope scope)
        {
            var @lock = _distributedLockProvider.CreateLock(CacheLockKey);
            await using (await @lock.AcquireAsync())
            {
                var key = Key;
                var val = await _cache.GetStringAsync(key);

                if (!string.IsNullOrEmpty(val))
                {
                    await _cache.SetStringAsync(key, val, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(KeyTimeout)
                    });
                }
                else
                {
                    var machineId = await CreateMachineIdAsync(LocalIp, MachineId);
                    if (machineId != null)
                    {
                        MachineId = machineId.Value;
                        Snowflake.SnowFlake.SetMachineId(MachineId);
                    }
                }

                return true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncInternal();
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncInternal()
        {
            var key = Key;
            await _cache.RemoveAsync(key);
        }

        public override void Dispose()
        {
            var key = Key;
            _cache.Remove(key);

            base.Dispose();
        }
    }
}
