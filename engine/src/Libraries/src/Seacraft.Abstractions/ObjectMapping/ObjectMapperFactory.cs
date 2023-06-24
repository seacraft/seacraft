// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'ObjectMapperFactory.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seacraft.Abstractions.ObjectMapping
{
    public class ObjectMapperFactory
    {
        private static IObjectMapper _objectMapper { get; set; }
        public static void SetObjectMapper(IObjectMapper objectMapper)
        {
            _objectMapper = objectMapper;
        }
        public static IObjectMapper GetObjectMapper()
        {
            return _objectMapper;
        }
    }
}
