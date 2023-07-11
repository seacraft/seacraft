// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftIdentityOptions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server.Configurations.IdentityServer.Options
{
    public class SeacraftIdentityOptions
    {
        public bool AllowedCreateUserFromExternalProvider = true;
        public bool AutoCreateUserFromExternalProvider = false;
        public bool AllowMultiLogin = false;
        public double SessionLiveTime = -1;
        public bool UseImageCaptcha = false;
        public double PasswordValidTime = -1;
        public bool UseStrictSecureTrans = false;
        public string DefaultPassword = "seacraft123";
        public int DefaultExpiredTime = -1;
        public bool AutoSavePassword = false;
        public string Title = "Seacraft用户中心服务";
        public string Theme = "light";
        public string Desc = "";
        public bool UseImageInLoginPage = false;
        public bool ShowSupportInfoInLoginPage = false;
        public string SupportInfo = "技术支持：Seacraft";
        public string DefaultAuthenticationSchema = "local";
        public bool EnableOfflineMode = false;
        public bool EnableLocalAuthentication = true;
        public bool PasswordRequireNotEqualToUsername { get; set; } = false;
    }
}
