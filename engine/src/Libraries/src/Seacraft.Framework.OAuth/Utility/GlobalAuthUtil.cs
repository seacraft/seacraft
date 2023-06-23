// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'GlobalAuthUtil.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Seacraft.Framework.OAuth
{
    public static class GlobalAuthUtil
    {
        public static string ParseMapToString(Dictionary<string, object> dictParams)
        {
            if (dictParams is  null)
            {
                throw new ArgumentNullException(nameof(dictParams));
            }
            var builder = new StringBuilder();
            if (dictParams.Any())
            {
                builder.Append("");
                int i = 0;
                foreach (KeyValuePair<string, object> item in dictParams)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, Convert.ToString(item.Value));
                    i++;
                }
            }
            return builder.ToString();
        }


        public static bool IsHttpProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            return url.StartsWith("http://");
        }

        public static bool IsHttpsProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            return url.StartsWith("https://");
        }
        public static bool IsLocalHost(string url)
        {
            return string.IsNullOrWhiteSpace(url) || url.Contains("127.0.0.1") || url.Contains("localhost");
        }

        public static string UrlEncode(string value)
        {
            if (value is null)
            {
                return "";
            }
            try
            {

                return System.Web.HttpUtility.UrlEncode(value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed To Encode Uri", e);
            }
        }


        public static string UrlDecode(string value)
        {
            if (value is null)
            {
                return "";
            }
            try
            {
                return HttpUtility.UrlDecode(value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed To Decode Uri", e);
            }
        }


        public static T EnumFromString<T>(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"No authorization type found: {type}");
            }
            try
            {
                T result = (T)Enum.Parse(typeof(T), type);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Authorization type resolution failed. Procedure: {type}", e);
            }
        }

        public static List<Dictionary<string, object>> ParseListObject(this string jsonStr)
        {
            var retDict = new List<Dictionary<string, object>>();
            if (!string.IsNullOrWhiteSpace(jsonStr)) retDict = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonStr);
            return retDict;

        }
        public static Dictionary<string, object> ParseObject(this string jsonStr)
        {
            var retDict = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(jsonStr)) retDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
            return retDict;
        }



        public static Dictionary<string, object> ParseUrlObject(this string paramsStr)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrWhiteSpace(paramsStr))
                {
                    return res;
                }
                // 去掉Path部分
                int pathEndPos = paramsStr.IndexOf('?');
                if (pathEndPos > -1)
                {
                    paramsStr = paramsStr.Substring(pathEndPos + 1);
                }

                return ParseStringObject(paramsStr);
            }
            catch (Exception e)
            {
                return res;
            }


        }
        public static Dictionary<string, object> ParseStringObject(this string accessTokenStr)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (accessTokenStr.Contains("&"))
            {
                string[] fields = accessTokenStr.Split("&");
                foreach (var field in fields)
                {
                    if (field.Contains("="))
                    {
                        string[] keyValue = field.Split("=");
                        res.Add(UrlDecode(keyValue[0]), keyValue.Length == 2 ? UrlDecode(keyValue[1]) : null);
                    }
                }
            }
            return res;
        }

        public static string SpellParams(this Dictionary<string, object> dictParams)
        {
            if (dictParams is null)
            {
                throw new ArgumentNullException(nameof(dictParams));
            }

            var builder = new StringBuilder();
            if (dictParams.Any())
            {
                builder.Append("");
                int i = 0;
                foreach (KeyValuePair<string, object> item in dictParams)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, Convert.ToString(item.Value));
                    i++;
                }
            }
            return builder.ToString();
        }

        public static string GetString(this Dictionary<string, object> dic, string key)
        {
            if (dic is null)
                return "";
            if (dic.TryGetValue(key, out object value))
            {
                return Convert.ToString(value);
            }
            else
            {
                return "";
            }
        }


        public static Dictionary<string, object> Sort(this Dictionary<string, object> dic, bool isAsc = true)
        {
            Dictionary<string, object> rdic = new Dictionary<string, object>();
            if (dic.Count > 0)
            {
                List<KeyValuePair<string, object>> lst = new List<KeyValuePair<string, object>>(dic);
                lst.Sort(delegate (KeyValuePair<string, object> s1, KeyValuePair<string, object> s2)
                {
                    if (isAsc)
                    {
                        return String.CompareOrdinal(s1.Key, s2.Key);
                    }
                    else
                    {
                        return String.CompareOrdinal(s2.Key, s1.Key);
                    }
                });

                foreach (KeyValuePair<string, object> kvp in lst)
                    rdic.Add(kvp.Key, kvp.Value);
            }
            return rdic;
        }
    }
}
