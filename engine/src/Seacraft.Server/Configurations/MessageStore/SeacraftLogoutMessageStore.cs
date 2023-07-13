// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftLogoutMessageStore.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Models;
using IdentityServer4.Stores;
using Seacraft.Server.Configurations.IdentityServer.Extentions;

namespace Seacraft.Server.Configurations.MessageStore
{
    public class SeacraftLogoutMessageStore : IMessageStore<LogoutMessage>
    {
        private readonly ISeacraftCache cache;
        public SeacraftLogoutMessageStore(ISeacraftCache cache)
        {
            this.cache = cache;
        }
        public async Task<Message<LogoutMessage>> ReadAsync(string id)
        {
            var result = this.cache.Get<Message<LogoutMessage>>(id);
            return result;
        }

        public async Task<string> WriteAsync(Message<LogoutMessage> message)
        {
            var key = Guid.NewGuid().ToString("N");
            this.cache.Set(key, message, 600);
            return key;
        }
    }
}
