// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'MyIntrospectionResponseGenerator.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace Seacraft.Server.Configurations.IdentityServer.ResponseHandling
{
    public class MyIntrospectionResponseGenerator : IntrospectionResponseGenerator
    {
        public MyIntrospectionResponseGenerator(IEventService events, ILogger<IntrospectionResponseGenerator> logger) : base(events, logger)
        {
        }
        protected override Task<bool> AreExpectedScopesPresentAsync(IntrospectionRequestValidationResult validationResult)
        {
            if (validationResult.Api.Properties.TryGetValue("IsSuperior", out string superior) && superior == "true")
            {
                return Task.FromResult(true);
            }
            return base.AreExpectedScopesPresentAsync(validationResult);
        }
    }
}
