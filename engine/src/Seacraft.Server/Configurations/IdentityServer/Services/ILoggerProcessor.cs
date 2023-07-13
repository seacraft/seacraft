// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ILoggerProcessor.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Events;

namespace Seacraft.Server.Configurations.IdentityServer.Services
{
    public interface ILoggerProcessor
    {
        void EnqueueEvent(Event evt);
    }
}
