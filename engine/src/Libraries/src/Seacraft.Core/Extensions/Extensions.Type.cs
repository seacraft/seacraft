// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'Extensions.Type.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Core.Extensions
{
    /// <summary>
    /// Type <see cref="Type"/> Auxiliary extension method class
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// IsNullableType is Check whether the type is Nullable type 
        /// </summary>
        /// <param name="type"> The type to be processed </param>
        /// <returns> It's return false, not return true </returns>
        public static bool IsNullableType(this Type type)
        {
            return ((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// GetNonNummableType is Returns the actual type by the Nullable type of the type
        /// </summary>
        /// <param name="type"> Type object to handle </param>
        /// <returns> </returns>
        public static Type GetNonNummableType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// GetUnNullableType Get the base type of the Nullable type using the type converter
        /// </summary>
        /// <param name="type"> Type object to handle </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (!IsNullableType(type))
            {
                return type;
            }

            var nullableConverter = new NullableConverter(type);
            return nullableConverter.UnderlyingType;
        }

        /// <summary>
        /// ToTypeDescription is  Description Gets the feature description of the type
        /// </summary>
        /// <param name="type">Type object</param>
        /// <param name="inherit">Whether to search a type's inheritance chain to find a descriptive property</param>
        /// <returns>Description Indicates the feature description, or the full name of the type if it does not exist</returns>
        public static string ToTypeDescription(this Type type, bool inherit = false)
        {
            var desc = type.GetAttribute<DescriptionAttribute>(inherit);
            return desc == null ? type.FullName : desc.Description;
        }

        /// <summary>
        ///ToDescription is Get the Description feature description of the member metadata
        /// </summary>
        /// <param name="member">Member metadata object</param>
        /// <param name="inherit">Whether to search a member's inheritance chain to find a descriptive feature</param>
        /// <returns>Description Indicates the feature description or the member name if it does not exist</returns>
        public static string ToDescription(this MemberInfo member, bool inherit = false)
        {
            var desc = member.GetAttribute<DescriptionAttribute>(inherit);
            if (desc != null)
            {
                return desc.Description;
            }
            var displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            var display = member.GetAttribute<DisplayAttribute>(inherit);
            if (display != null)
            {
                return display.Name;
            }
            return member.Name;
        }


        /// <summary>
        /// HasAttribute is Checks whether the specified Attribute attribute exists in the member of the specified type
        /// </summary>
        /// <typeparam name="T">Attribute type to check</typeparam>
        /// <param name="memberInfo">Type member to check</param>
        /// <param name="inherit">Whether to look up from inheritance</param>
        /// <returns>Existence or not</returns>
        public static bool HasAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.IsDefined(typeof(T), inherit);
            //return memberInfo.GetCustomAttributes(typeof(T), inherit).Any(m => (m as T) != null);
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            var descripts = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return descripts.FirstOrDefault() as T;
        }


        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        /// 判断类型是否为集合类型
        /// </summary>
        /// <param name="type">要处理的类型</param>
        /// <returns>是返回True，不是返回False</returns>
        public static bool IsEnumerable(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// 方法是否是异步
        /// </summary>
        public static bool IsAsync(this MethodInfo method)
        {
            return method.ReturnType == typeof(Task)
                   || method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }


        /// <summary>
        /// 获取指定类型的所有子类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isClass"></param>
        /// <param name="isAbstract"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubTypes(this Type type, bool isClass = true, bool isAbstract = false)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(it => it.GetTypes())
                .Where(it => it.IsClass == isClass && it.IsAbstract == isAbstract &&
                             type.IsAssignableFrom(it));
        }


    }
}
