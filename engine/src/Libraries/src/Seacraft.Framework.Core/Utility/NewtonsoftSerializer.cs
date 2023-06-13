// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
