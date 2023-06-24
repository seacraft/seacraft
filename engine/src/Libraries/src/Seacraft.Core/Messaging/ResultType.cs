// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ResultType.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Seacraft.Core.Messaging
{
    /// <summary>
    /// http status code
    /// </summary>
    public enum ResultType
    {

        [Description("Unauthorized information")]
        Info = 203,

        [Description("OK")]
        Success = 200,

        [Description("Internal server error")]
        Error = 500,

        [Description("Error occurred while encrypting the user password")]
        UnAuth = 401,

        [Description("Permission denied")]
        Forbidden = 403,

        [Description("Page not found")]
        NoFound = 404,
    }
}
