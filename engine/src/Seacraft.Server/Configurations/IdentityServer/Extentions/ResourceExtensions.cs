// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ResourceExtensions.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seacraft.Server.Configurations.IdentityServer.Extentions
{
    public static class ResourceExtensions
    {
        internal static ICollection<string> FindMatchingSigningAlgorithms(this IEnumerable<ApiResource> apiResources)
        {
            var apis = apiResources.ToList();

            if (apis.IsNullOrEmpty())
            {
                return new List<string>();
            }

            // only one API resource request, forward the allowed signing algorithms (if any)
            if (apis.Count == 1)
            {
                return apis.First().AllowedAccessTokenSigningAlgorithms;
            }

            var allAlgorithms = apis.Where(r => r.AllowedAccessTokenSigningAlgorithms.Any()).Select(r => r.AllowedAccessTokenSigningAlgorithms).ToList();

            // resources need to agree on allowed signing algorithms
            if (allAlgorithms.Any())
            {
                var allowedAlgorithms = IntersectLists(allAlgorithms);

                if (allowedAlgorithms.Any())
                {
                    return allowedAlgorithms.ToHashSet();
                }

                throw new InvalidOperationException("Signing algorithms requirements for requested resources are not compatible.");
            }

            return new List<string>();
        }

        private static IEnumerable<T> IntersectLists<T>(IEnumerable<IEnumerable<T>> lists)
        {
            return lists.Aggregate((l1, l2) => l1.Intersect(l2));
        }
    }
}
