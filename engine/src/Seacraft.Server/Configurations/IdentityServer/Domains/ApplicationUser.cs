// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ApplicationUser.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Seacraft.Server.Configurations.IdentityServer.Domains
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int SubjectId { get; set; }

        public ActivityStatus Status { get; set; } = ActivityStatus.Active;

        public DateTime ExpiredTime { get; set; }

        public GenderType Gender { get; set; } = GenderType.Unknown;

        public string Name { get; set; }

        public string SecurityQuestion { get; set; }

        public int SecurityAnswer { get; set; }

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; } = new List<IdentityUserClaim<int>>();
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; } = new List<IdentityUserLogin<int>>();
        public virtual ICollection<ApplicationUserRole> Roles { get; set; } = new List<ApplicationUserRole>();

        /// <summary>
        /// 密码过期时间
        /// </summary>
        public DateTime PasswordExpiredTime { get; set; }
        /// <summary>
        /// 是否需要重置密码
        /// </summary>
        public bool NeedResetPassword { get; set; }

        //public IList<IdentityUserClaim<int>> Claims { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }

    public class ApplicationRole : IdentityRole<int>
    {
        /// <summary>
        /// 角色代号（唯一）
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; } = RoleType.Normal;

        public string Application { get; set; }

        public string Source { get; set; }

        public virtual ICollection<ApplicationUserRole> Users { get; set; } = new List<ApplicationUserRole>();

    }

    public enum RoleType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal,

        /// <summary>
        /// 科室
        /// </summary>
        Department,

    }

    public enum GenderType
    {
        Default = 0,
        /// <summary>
        /// 男性
        /// </summary>
        Male = 1,
        /// <summary>
        /// 女性
        /// </summary>
        Female = 2,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 3,
    }

    public enum ActivityStatus
    {
        Unknown = 0,
        /// <summary>
        /// 审核中
        /// </summary>
        InReview = 1,

        /// <summary>
        /// 禁止
        /// </summary>
        Forbidden = 2,

        /// <summary>
        /// 激活
        /// </summary>
        Active = 3,

        /// <summary>
        /// 丢弃
        /// </summary>
        Obsolete = 4
    }
}
