// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'IPagedListRequest.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Framework.Core.Messaging
{
    /// <summary>
    /// The paging list request parameters
    /// </summary>
    public interface IPagedListRequest : IRequest
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
        /// The Skip how many pages
        /// </summary>
        int Skip { get; }
    }
}
