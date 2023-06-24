// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'SeacraftDbContext.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.EntityFrameworkCore;

namespace Seacraft.Repositories
{

    public class SeacraftDbContext
        : DbContext
    {
        public SeacraftDbContext(DbContextOptions options)
            : base(options)
        {
        }

    }

}