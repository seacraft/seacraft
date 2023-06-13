// Copyright 2023 Seacraft
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
    /// This is the paging query result parameter
    /// </summary>
    public class BasePagedListResult: IResult,IPagedListResult
    {
        /// <summary> 
        /// set  PageIndex and PageSize 
        /// </summary>
        /// <param name="request"> paging list request parameters</param>
        public void SetPageIndexAndPageSize(IPagedListRequest request)
        {
            this.PageIndex = request.PageIndex;
            this.PageSize = request.PageSize;
        }

        /// <summary>
        /// The current page is the first page by default
        /// </summary>

        private int _pageIndex = 1;
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
        /// number of total pages
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalItemCount < 1)
                {
                    return 0;
                }
                if (PageSize <= 1)
                {
                    return TotalItemCount;
                }
                var pageCount = TotalItemCount / PageSize;
                if (TotalItemCount % PageSize > 0)
                {
                    pageCount++;
                }
                return pageCount;
            }
        }

        /// <summary>
        /// total number
        /// </summary>
        private int _totalItemCount = 0;
        public int TotalItemCount
        {
            get
            {
                return _totalItemCount;
            }
            set
            {
                var val = value;
                if (val > 0)
                {
                    _totalItemCount = val;
                }
            }
        }
    }


}
