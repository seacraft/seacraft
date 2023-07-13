// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'DefaultPasswordGenerator.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System.Security.Cryptography;

namespace Seacraft.Server.Configurations.IdentityServer.Services.Default
{
    public class DefaultPasswordGenerator : IPasswordGenerator
    {
        public string Generate(int minLength, bool hasUpper, bool hasLowercase, bool hasNonAlphanumeric)
        {
            byte[] b = RandomNumberGenerator.GetBytes(4);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            var s = new List<string>();
            var nonAlpha = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            var num = "0123456789";
            var lower = "abcdefghijklmnopqrstuvwxyz";
            var upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < minLength; i++)
            {
                s.Add(num.Substring(r.Next(0, num.Length - 1), 1));
            }
            for (int i = 0; i < 3; i++)
            {
                if (hasUpper) { s.Add(upper.Substring(r.Next(0, upper.Length - 1), 1)); }
                if (hasLowercase) { s.Add(lower.Substring(r.Next(0, lower.Length - 1), 1)); }
                if (hasNonAlphanumeric) { s.Add(nonAlpha.Substring(r.Next(0, nonAlpha.Length - 1), 1)); }
            }
            s.Sort((x, y) => { return new Random().Next(-1, 1); });
            return String.Join("", s);
        }
    }
}
