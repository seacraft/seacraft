// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ChangePasswordRequest.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Models;

namespace Seacraft.Server.Configurations.IdentityServer.Models
{
    public class ChangePasswordRequest
    {
        public string ReturnUrl { get; set; }
        public LogoutMessage LogoutMessage { get; set; }
        public string State { get; set; }
        public bool NeedLogout { get; set; } = true;
        public string UserName { get; set; } = "";
        public bool NeedOldPassword { get; set; } = true;
    }
}
