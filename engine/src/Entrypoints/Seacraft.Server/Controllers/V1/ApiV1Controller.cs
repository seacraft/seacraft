// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ApiV1Controller.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Seacraft.Server.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class ApiV1Controller : Controller
    {

    }
}
