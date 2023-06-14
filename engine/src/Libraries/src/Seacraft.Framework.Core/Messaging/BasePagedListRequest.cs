// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'BasePagedListRequest.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

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
