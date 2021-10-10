using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    public interface IWeatherAlertsRepository
    {
        WeatherAlertFullInfo GetByLocation(Location location);
        WeatherAlertFullInfo Add(WeatherAlertFullInfo weatherAlertFullInfo);
        WeatherAlertFullInfo UpdateAlert(WeatherAlertFullInfo location);
    }

    public class WeatherAlertsRepository : IWeatherAlertsRepository
    {
        private readonly ILogger<WeatherAlertsRepository> _logger;

        private Faker _faker = new Faker();


        public WeatherAlertsRepository(ILogger<WeatherAlertsRepository> logger)
        {
            _logger = logger;
        }


        public WeatherAlertFullInfo GetByLocation(Location location)
        {
            if (_faker.Random.Bool(0.8f))
            {
                var randomSources = Enumerable.Range(0, _faker.Random.Int(0, 3))
                    .Select(_ => _faker.Internet.DomainName())
                    .ToList();
                return new(location, new List<WeatherAlert>(), randomSources);
            }

            return null;
        }

        public WeatherAlertFullInfo Add(WeatherAlertFullInfo weatherAlertFullInfo)
        {
            _logger.LogInformation("Added and propagated new alert info");

            return weatherAlertFullInfo;
        }

        public WeatherAlertFullInfo UpdateAlert(WeatherAlertFullInfo weatherAlertFullInfo)
        {
            _logger.LogInformation("Updates to alert info are saved and propagated");

            return weatherAlertFullInfo;
        }
    }

    public record WeatherAlertFullInfo(Location Location, List<WeatherAlert> Alerts, List<string> Sources);
}