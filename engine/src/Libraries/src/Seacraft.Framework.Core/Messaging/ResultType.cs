// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Seacraft.Framework.Core.Messaging
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
