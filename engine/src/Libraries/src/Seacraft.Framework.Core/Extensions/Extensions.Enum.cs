// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Framework.Core
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
