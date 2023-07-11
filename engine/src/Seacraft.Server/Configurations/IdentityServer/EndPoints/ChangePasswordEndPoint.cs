// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ChangePasswordEndPoint.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Configuration;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Seacraft.Server.Configurations.IdentityServer.Extentions;
using Seacraft.Server.Configurations.IdentityServer.Models;
using Seacraft.Server.Configurations.IdentityServer.Storage;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text.Encodings.Web;

namespace Seacraft.Server.Configurations.IdentityServer.EndPoints
{
    public class ChangePasswordEndPoint : IEndpointHandler
    {
        private readonly IEndSessionRequestValidator _endSessionRequestValidator;
        private readonly ILogger<ChangePasswordEndPoint> logger;
        private readonly IUserSession _userSession;
        public ChangePasswordEndPoint(
            ILogger<ChangePasswordEndPoint> logger,
            IUserSession userSession,
            IEndSessionRequestValidator endSessionRequestValidator)
        {
            this.logger = logger;
            _userSession = userSession;
            _endSessionRequestValidator = endSessionRequestValidator;
        }
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            NameValueCollection parameters;
            if (HttpMethods.IsGet(context.Request.Method))
            {
                // state
                // redirect_url
                // id_token_hint
                parameters = context.Request.Query.AsNameValueCollection();
                var redirect_uri = parameters.Get("redirect_uri");
                var id_token_hint = parameters.Get("id_token_hint");
                var state = parameters.Get("state");
                if (!string.IsNullOrEmpty(redirect_uri))
                {
                    parameters.Add("post_logout_redirect_uri", redirect_uri);
                }
                var user = await _userSession.GetUserAsync();
                if (user == null)
                {
                    return new ErrorPageResult("用户未登录");
                }
                if (string.IsNullOrEmpty(redirect_uri) && string.IsNullOrEmpty(id_token_hint))
                {
                    logger.LogError("Not Found redirect_uri or id_token_hint");
                    return new ErrorPageResult("参数错误");
                }
                EndSessionValidationResult result = null;
                if (!string.IsNullOrEmpty(id_token_hint))
                {
                    result = await _endSessionRequestValidator.ValidateAsync(parameters, user);
                }
                return new ChangePasswordResult(result, redirect_uri, state ?? "");
            }
            else
            {
                logger.LogWarning("Invalid HTTP method for end session endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

        }
    }

    public class ErrorPageResult : IEndpointResult
    {
        private readonly string _message;
        private readonly string _detail;
        public ErrorPageResult(string message, string detail = "")
        {
            _message = message;
            _detail = detail;
        }
        public async Task ExecuteAsync(HttpContext context)
        {
            var _options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
            var _errorMessageStore = context.RequestServices.GetRequiredService<IMessageStore<ErrorMessage>>();
            var errorModel = new ErrorMessage
            {
                RequestId = context.TraceIdentifier,
                Error = _message,
            };
            var message = new Message<ErrorMessage>(errorModel, DateTime.UtcNow);
            var id = await _errorMessageStore.WriteAsync(message);
            var errorUrl = _options.UserInteraction.ErrorUrl;
            var url = errorUrl.AddQueryString(_options.UserInteraction.ErrorIdParameter, id);
            context.Response.RedirectToAbsoluteUrl(url);
        }
    }



    public class ChangePasswordResult : IEndpointResult
    {
        private readonly EndSessionValidationResult _result;            // 使用EndSession Flow
        private readonly string _returnUrl;                             // 单纯返回
        private readonly string _state;
        public ChangePasswordResult(EndSessionValidationResult request, string returnUrl, string state = "")
        {
            _state = state;
            _result = request;
            _returnUrl = returnUrl;
        }
        public async Task ExecuteAsync(HttpContext context)
        {
            var store = context.RequestServices.GetRequiredService<ChangePasswordRequestStore>();
            var validatedRequest = _result != null && _result.IsError == false ? _result.ValidatedRequest : null;
            var request = new ChangePasswordRequest { ReturnUrl = _returnUrl, State = _state };
            if (validatedRequest != null)
            {
                request.LogoutMessage = new LogoutMessage(validatedRequest);
            }
            string id = await store.Store(request);

            // TODO: 塞到Options里
            var redirect = "/Account/ChangePassword";
            if (redirect.IsLocalUrl())
            {
                redirect = context.GetIdentityServerRelativeUrl(redirect);
            }
            if (id != null)
            {
                // TODO: 塞到Options里
                redirect = redirect.AddQueryString("changePasswordId", id);
            }
            // 跳转到changePassword
            context.Response.Redirect(redirect);
        }
    }
}
