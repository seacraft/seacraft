// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'BasePagedListResult.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Core.Messaging
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
