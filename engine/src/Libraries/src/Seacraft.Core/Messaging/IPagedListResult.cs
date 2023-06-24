// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IPagedListResult.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Core.Messaging
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
