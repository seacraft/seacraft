// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IPasswordGenerator.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server.Configurations.IdentityServer.Services
{
    public interface IPasswordGenerator
    {
        string Generate(int minLength, bool hasUpper, bool hasLowercase, bool hasNonAlphanumeric);
    }
}
