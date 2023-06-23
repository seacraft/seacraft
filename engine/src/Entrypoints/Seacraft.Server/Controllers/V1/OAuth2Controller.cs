// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'OAuth2Controller.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seacraft.Framework.Core.Messaging;

namespace Seacraft.Server.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuth2Controller : ApiV1Controller
    {
        public OAuth2Controller() 
        {
               
        }

        [AllowAnonymous]
        [HttpGet]
        public ServiceResult Authorization(string source) 
        {
            
        }

        public ServiceResult Callback() 
        {

        }
    }
}
