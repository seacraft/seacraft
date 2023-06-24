// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IdGenerationService.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake;

namespace Seacraft.SnowFlake.Distributed
{
    public class DefaultGeneration : IDefaultGeneration
    {
        private readonly Snowflake.SnowFlake _snowFlake;

        public DefaultGeneration(Snowflake.SnowFlake snowFlake)
        {
            _snowFlake = snowFlake;
        }

        public long GenerateNewDistributedId()
        {
           return  _snowFlake.NextId();
        }

        public Task<long> GenerateNewDistributedIdAsync(CancellationToken? token = null)
        {
            var id = _snowFlake.NextId();
            return Task.FromResult(id);
        }
    }

    public interface IDefaultGeneration
    {
        public Task<long> GenerateNewDistributedIdAsync(CancellationToken? token = null);


        public long GenerateNewDistributedId();

    }
}
