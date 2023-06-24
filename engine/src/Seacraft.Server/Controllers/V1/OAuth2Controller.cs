// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'OAuth2Controller.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seacraft.Core.Messaging;

namespace Seacraft.Server.Controllers.V1
{
    public class OAuth2Controller : ApiV1Controller
    {
        private const string GitLabAuth = "https://gitlab.com/oauth/authorize";


        public OAuth2Controller() 
        {
               
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public ServiceResult Authorization(string source) 
        //{
        //    UrlBuilder.FromBaseUrl(GitLabAuth)
        //   .queryParam("response_type", "code")
        //   .queryParam("client_id", config.clientId)
        //   .queryParam("redirect_uri", config.redirectUri)
        //   .queryParam("state", getRealState(state))
        //   .queryParam("scope", config.scope.IsNullOrWhiteSpace() ? "read_user+openid+profile+email" : config.scope)
        //   .build();
        //}

        //public ServiceResult Callback() 
        //{

        //}
    }
}
