// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IdService.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Snowflake;

namespace Seacraft.Services
{

    public class IdService
        : IIdService
    {

        private readonly SnowFlake _snowFlake;

        public IdService(SnowFlake snowFlake)
        {
            this._snowFlake = snowFlake;
        }

        public Task<long> GetDistributedIdAsync(CancellationToken? token = null)
        {
            var id = this._snowFlake.NextId();
            return Task.FromResult(id);
        }

    }

}
