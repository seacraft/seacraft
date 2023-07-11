// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'UseMemoryBuilder.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.IdentityServer4
{
    public class UseMemoryStoreBuilder
    {
        private readonly MemoryStoreOptions _options;

        public UseMemoryStoreBuilder(MemoryStoreOptions options) 
        {
          if (options is null) throw new ArgumentNullException(nameof(options));        
        }

        public void AddMemoryApiScopes() 
        {
              
        }


    }
}
