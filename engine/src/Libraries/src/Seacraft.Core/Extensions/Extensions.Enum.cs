// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'Extensions.Enum.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Core.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// 获取成员的Description特性描述信息
        /// </summary>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string ToDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            Type type = value.GetType().GetNonNummableType();

            MemberInfo member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.ToDescription() : value.ToString();
        }

        /// <summary>
        /// 将字符串转换为枚举值。
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">要转换的字符串值</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>返回枚举的值</returns>
        public static T ToEnum<T>(this object value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T)Enum.Parse(typeof(T), value.ToString(), ignoreCase);
        }

        /// <summary>
        /// 返回枚举的全部成员描述数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<string> ToDescriptions<T>() where T : struct
        {
            var type = typeof(T);
            var names = Enum.GetNames(type);

            return names.Select(item => {
                var member = type.GetMember(item).FirstOrDefault();
                return member != null ? member.ToDescription() : member.ToString();
            }).ToList();
        }
    }
}
