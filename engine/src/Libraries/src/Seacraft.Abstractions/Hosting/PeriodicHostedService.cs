// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'PeriodicHostedService.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using   Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seacraft.Abstractions.Hosting
{
    public abstract class PeriodicHostedService
       : BackgroundService
    {

        private readonly ILogger<PeriodicHostedService> _logger;
        private readonly IServiceScopeFactory _factory;

        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);

        public bool IsEnabled { get; set; }

        public virtual TimeSpan Period
        {
            get
            {
                return _period;
            }
        }

        public PeriodicHostedService(
            ILogger<PeriodicHostedService> logger,
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected abstract Task<bool> ExecuteWithinAsync(AsyncServiceScope scope);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (this.IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        var @continue = await this.ExecuteWithinAsync(asyncScope);
                        if (@continue)
                        {
                            this._logger.LogInformation($"Executed PeriodicHostedService: {this.GetType().FullName}");
                            continue;
                        }
                    }

                    this._logger.LogInformation("Skipped PeriodicHostedService");
                }
                catch (Exception ex)
                {
                    this._logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }
}
