using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using ValorantClient.Lib.API.Regions;
using ValorantClient.Lib.Caching;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Network;
using ValorantClient.Lib.RiotFiles.Lock;

namespace ValorantClient.Lib.Client
{
    /// <summary>
    /// Implementation of <see cref="IValoClientBuilder"/>
    /// </summary>
    public class ValoClientBuilder : IValoClientBuilder
    {

        private readonly IServiceCollection _services = new ServiceCollection();

        public ValoClientBuilder()
        {
            _services.AddSingleton<ConfigOptions>();
            _services.AddSingleton<IConfiguration, JsonConfiguration>();
            _services.AddTransient<IHttpClientService, HttpClientService>();
            _services.AddSingleton<ILockFileService, LockFileService>();
            _services.AddSingleton<ICache, Cache>();
            _services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }

        public IServiceCollection Services => _services;

        public async Task<(ServiceProvider Provider, IMediator Mediator)> BuildAsync(string region = "eu")
        {
            var provider = _services.BuildServiceProvider();

            var configService = provider.GetRequiredService<IConfiguration>();
            var configOptions = provider.GetRequiredService<ConfigOptions>();
            await configService.LoadConfigFileAsync(configOptions.Path);

            var mediator = provider.GetRequiredService<IMediator>();
            await mediator.Send(new SetRegionCommand(region, region));

            return (Provider: provider,Mediator: mediator);
        }
    }
}
