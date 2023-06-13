// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Framework.Core.Messaging
{
    /// <summary>
    /// This is the paging query request parameter
    /// </summary>
    public class BasePagedListRequest : IPagedListRequest
    {
        private int _pageIndex = 1;

        /// <summary>
        /// The current page is the first page by default
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                var val = value;
                if (val > 0)
                {
                    _pageIndex = val;
                }
            }
        }

        private int _pageSize = 10;

        /// <summary>
        /// The default number of items per page is ten
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                var val = value;
                if (val > 0)
                {
                    _pageSize = val;
                }
            }
        }

        /// <summary>
        /// The Skip how many pages
        /// </summary>
        public int Skip
        {
            get
            {
                return (this.PageIndex - 1) * this.PageSize;
            }
        }
    }
}
