using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    public interface IWeatherAlertsHelper
    {
        IEnumerable<Location> GetServedLocations(string source);
        IEnumerable<WeatherAlert> GetAlerts (string source, Location location);
        void SetupPoller(string source, Location location);
        void TriggerAlertsNotification();
    }

    public class WeatherAlertsHelper : IWeatherAlertsHelper
    {
        private readonly ILogger<WeatherAlertsHelper> _logger;

        private Faker _faker = new Faker();


        public WeatherAlertsHelper(ILogger<WeatherAlertsHelper> logger)
        {
            _logger = logger;
        }


        public IEnumerable<Location> GetServedLocations(string source)
        {
            if (source == "example.com") throw new Exception("Can't parse response"); // wrong source
            if (source == "demo2.openweatheralerts.com") throw new Exception("Connection refused"); // dead source
            if (source == "us.v2.openweatheralerts.com") throw new Exception("Can't parse response"); // source with different schema

            return Enumerable.Range(0, _faker.Random.Int(1, 15))
                .Select(_ => new Location(_faker.Address.Country(), _faker.Address.City()));
        }

        public IEnumerable<WeatherAlert> GetAlerts(string source, Location location)
        {
            return Enumerable.Range(0, _faker.Random.Int(0, 5))
                .Select(_ => new WeatherAlert(_faker.Date.Soon(14), _faker.Random.Int(1, 7), _faker.Hacker.Phrase()));
        }

        public void SetupPoller(string source, Location location)
        {
            _logger.LogInformation("Pollers are updated");
        }

        public void TriggerAlertsNotification()
        {
            _logger.LogInformation("Alerting job {AlertJobId} was triggered", Guid.NewGuid());
        }
    }

    public record Location(string Country, string City);

    public record WeatherAlert(DateTime DateTime, int Severity, string Description);
}