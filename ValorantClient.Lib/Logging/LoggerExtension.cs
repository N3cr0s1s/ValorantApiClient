using Microsoft.Extensions.DependencyInjection;

namespace ValorantClient.Lib.Logging
{
    public static class LoggerExtension
    {

        /// <summary>
        /// Add <see cref="ILogger{T}"/> to services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            LoggerOptions options = new LoggerOptions();
            services.AddSingleton(options);

            if (options.Disable) 
                return services;

            services.AddTransient(typeof(ILogger<>), typeof(ConsoleLogger<>));
            return services;
        }

        /// <summary>
        /// Add <see cref="ILogger{T}"/> with <see cref="LoggerOptions"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogger(this IServiceCollection services,Action<LoggerOptions> optionsAction)
        {
            LoggerOptions options = new LoggerOptions();
            optionsAction(options);
            services.AddSingleton(options);

            if (options.Disable)
                return services;

            services.AddTransient(typeof(ILogger<>), options.Logger);
            return services;
        }
    }
}
