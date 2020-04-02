using Microsoft.Extensions.DependencyInjection;

namespace Banking.Enterprise.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Configure<T>(this IServiceCollection services) where T : class
        {
            services.AddSingleton<IConfigOptions<T>, ConfigOptions<T>>();
          
        }
    }
}
