// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'DesignTimeDbContextFactory.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Seacraft.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seacraft.Repositories
{

    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<SeacraftDbContext>
    {

        public SeacraftDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.Development.json", true, true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration["ConnectionStrings:Sqlite"];
            var optionsBuilder = new DbContextOptionsBuilder<SeacraftDbContext>();
            optionsBuilder.UseSqlite(connectionString,
                x => x.MigrationsAssembly(typeof(SeacraftDbContext).Assembly.FullName));

            return new SeacraftDbContext(optionsBuilder.Options);
        }

    }

}
