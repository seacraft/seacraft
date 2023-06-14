// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'NewtonsoftSerializer.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Framework.Core.Utility
{
    /// <summary>
    /// The serialization and deserialization tools
    /// </summary>
    public class NewtonsoftSerializer
    {
        /// <summary>
        /// Encoding to use to convert string to byte[] and the other way around.
        /// </summary>
        /// <remarks>
        /// StackExchange.Redis uses Encoding.UTF8 to convert strings to bytes,
        /// hence we do same here.
        /// </remarks>
        private static readonly Encoding encoding = Encoding.UTF8;

        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftSerializer"/> class.
        /// </summary>
        public NewtonsoftSerializer() : this(null)
        {
            this.settings = settings ?? new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftSerializer"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public NewtonsoftSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings ?? new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public byte[] Serialize(object item)
        {
            var type = item?.GetType();
            var jsonString = JsonConvert.SerializeObject(item, type, settings);
            return encoding.GetBytes(jsonString);
        }

        /// <summary>
        ///  Serializes the specified item to camel case
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string SerializeDefaultCamelCase(object item, bool indented = false)
        {
            var type = item?.GetType();
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            if (indented)
            {
                setting.Formatting = Formatting.Indented;
            }
            var jsonString = JsonConvert.SerializeObject(item, type, setting);
            return jsonString;
        }

        /// <summary>
        ///  Serializes the specified item to camel case
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string SerializeCamelCase(object item, bool indented = false)
        {
            var type = item?.GetType();

            var setting = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            if (indented)
            {
                setting.Formatting = Formatting.Indented;
            }
            var jsonString = JsonConvert.SerializeObject(item, type, setting);
            return jsonString;
        }

        public string SerializeDefault(object item)
        {
            return JsonConvert.SerializeObject(item);
        }

        /// <summary>
        ///  Serializes the specified item to camel case
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string Serialize(object item, IContractResolver contractResolver, bool indented = false)
        {
            var type = item?.GetType();

            var setting = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            if (indented)
            {
                setting.Formatting = Formatting.Indented;
            }
            var jsonString = JsonConvert.SerializeObject(item, type, setting);
            return jsonString;
        }


        public string SerializeIndentedCamelCase(object item)
        {
            return SerializeDefaultCamelCase(item, true);

        }

        public static T Deserialize<T>(
            string serialized,
            bool indented = false)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }
            return JsonConvert.DeserializeObject<T>(serialized, settings);
        }

        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public object Deserialize(byte[] serializedObject)
        {
            var jsonString = encoding.GetString(serializedObject);
            return JsonConvert.DeserializeObject(jsonString, typeof(object));
        }


        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] serializedObject)
        {
            var jsonString = encoding.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }
    }
}
