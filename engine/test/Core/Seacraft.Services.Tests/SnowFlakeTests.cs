// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'UnitTest1.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.Extensions.DependencyInjection;

namespace Seacraft.Services.Tests;

public class SnowFlakeTests
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IIdGenerationService _idService;

    public SnowFlakeTests()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddDistributedMemoryCache();

        services.AddMemoryDistributedLock();
        services.AddSnowFlakeService(snowFlake =>
        {
            snowFlake.Name = Guid.NewGuid().ToString();
            snowFlake.DataCenterId = 0;
        });

        this._serviceProvider = services.BuildServiceProvider();

        this._idService = this._serviceProvider.GetService<IIdGenerationService>();
    }

    [Fact]
    public async Task GetIdTest()
    {
        var list = new List<long>();
        for (int i = 0; i < 10; i++)
        {
            var id = await this._idService.GenerateNewDistributedIdAsync();
            Assert.True(id > 0);
            list.Add(id);
        }
        Assert.True(Enumerable.SequenceEqual(list, list.OrderBy(x => x)));
    }

}