using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Framework.OAuth
{
    public  class UrlBuilder
    {
        private Dictionary<string, object> paramDict = new Dictionary<string, object>();

        public string BaseUrl { get; private set; }

        public static UrlBuilder FromBaseUrl(string baseUrl)
        {
          return new UrlBuilder()
            {
              BaseUrl = baseUrl    
            };
        }


        public UrlBuilder QueryParam(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"{nameof(key)} not null");
            }

            string? valueAsString = value is null ? null : Convert.ToString(value);
            this.paramDict.Add(key, valueAsString);
            return this;
        }
        public string Build()
        {
            if (!this.paramDict.Any())
            {
                return this.BaseUrl;
            }
            string baseUrl = this.AppendIfNotContain(this.BaseUrl, "?", "&");
            string paramString = GlobalAuthUtil.ParseMapToString(this.paramDict);
            return baseUrl + paramString;
        }

        public override string ToString()
        {
            return this.Build();
        }


        public string AppendIfNotContain(string str, string appendStr, string otherwise)
        {
            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(appendStr))
            {
                return str;
            }
            if (str.Contains(appendStr))
            {
                return string.Concat(str, otherwise);
            }
            return string.Concat(str, appendStr);
        }
    }
}
