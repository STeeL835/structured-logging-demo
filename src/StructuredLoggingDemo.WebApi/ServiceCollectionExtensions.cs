using Microsoft.Extensions.DependencyInjection;
using StructuredLoggingDemo.WebApi.Emailing;
using StructuredLoggingDemo.WebApi.WeatherForecast;

namespace StructuredLoggingDemo.WebApi
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IWeatherForecastService, WeatherForecastService>()
                .AddSingleton<IWeatherAlertsHelper, WeatherAlertsHelper>()
                .AddScoped<IWeatherAlertsRepository, WeatherAlertsRepository>()
                .AddScoped<IWeatherAlertsService, WeatherAlertsService>()
                .AddScoped<IEmailService, EmailService>();
        }
    }
}