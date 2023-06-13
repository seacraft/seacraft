﻿// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Framework.Core.Messaging
{
    /// <summary>
    /// This is the paging list base class for returns
    /// </summary>
    public interface IPagedListResult
    {
        /// <summary>
        /// Current page number
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// How many pieces per page
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// number of total pages
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// total number
        /// </summary>
        int TotalItemCount { get; set; }
    }

}
