// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ChangePasswordRequestStore.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Seacraft.Server.Configurations.IdentityServer.Extentions;
using Seacraft.Server.Configurations.IdentityServer.Models;

namespace Seacraft.Server.Configurations.IdentityServer.Storage
{
    public class ChangePasswordRequestStore
    {
        private readonly ISeacraftCache cache;
        public ChangePasswordRequestStore(ISeacraftCache cache)
        {
            this.cache = cache;
        }

        public async Task<string> Store(ChangePasswordRequest request)
        {
            var key = Guid.NewGuid().ToString("N");
            cache.Set(key, request, 600);
            return key;
        }
        public async Task Remove(string key)
        {
            cache.Remove(key);
        }
        public async Task<ChangePasswordRequest> Retrive(string key)
        {
            // 反序列化的时候会出问题
            var result = cache.Get<ChangePasswordRequest>(key);
            return result;
        }
    }
}
