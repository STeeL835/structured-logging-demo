using Microsoft.Extensions.DependencyInjection;
using StructuredLoggingDemo.WebApi.Services;

namespace StructuredLoggingDemo.WebApi
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services.AddScoped<IWeatherForecastService, FakeWeatherForecastService>();
        }
    }
}