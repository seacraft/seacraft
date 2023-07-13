// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'LoggingEventSink.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seacraft.Server.Configurations.IdentityServer.Services
{
    public class LoggingEventSink : IEventSink
    {

        private readonly ILogger<LoggingEventSink> _logger;
        private readonly ILoggerProcessor processor;
        private readonly IWebHostEnvironment environment;
        public LoggingEventSink(ILogger<LoggingEventSink> logger, ILoggerProcessor processor, IWebHostEnvironment env)
        {
            _logger = logger;
            this.processor = processor;
            environment = env;
        }
        public Task PersistAsync(Event evt)
        {
            this.processor.EnqueueEvent(evt);
            switch (evt)
            {
                case ApiAuthenticationFailureEvent _:
                   
                    break;
                case ApiAuthenticationSuccessEvent _:
                 
                    break;
                case ClientAuthenticationSuccessEvent clientAuthSuccess:
                 
                    break;
                case ClientAuthenticationFailureEvent clientAuthFailure:
                  
                    break;
                case TokenIssuedSuccessEvent tokenIssuedSuccess:
              
                    break;
                case TokenIssuedFailureEvent tokenIssuedFailure:
               
                    break;
                case TokenIntrospectionSuccessEvent _:
                   
                    break;
                case TokenIntrospectionFailureEvent _:
                  
                    break;
                case TokenRevokedSuccessEvent _:
                  
                    break;
                case UserLoginSuccessEvent successEvent:
                 
                    break;
                case UserLoginFailureEvent _:
               
                    break;
                case UserLogoutSuccessEvent _:
                  
                    break;
                case ConsentGrantedEvent _:
                 
                    break;
                case ConsentDeniedEvent _:
                   
                    break;
                case UnhandledExceptionEvent _:
               
                    break;
                case DeviceAuthorizationSuccessEvent _:
              
                    break;
                case DeviceAuthorizationFailureEvent _:
                 
                    break;
            }
            if (environment.IsDevelopment())
            {
                _logger.LogDebug(System.Text.Json.JsonSerializer.Serialize(evt));
            }
            return Task.CompletedTask;
        }
    }

}