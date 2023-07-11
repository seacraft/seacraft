// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'DateTimeExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server.Configurations.IdentityServer.Extentions
{
    public static class DateTimeExtensions
    {
        public static bool HasExceeded(this DateTime creationTime, int seconds, DateTime now)
        {
            return (now > creationTime.AddSeconds(seconds));
        }
    }
}
