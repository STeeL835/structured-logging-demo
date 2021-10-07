using System;
using System.Collections.Generic;

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
        public IEnumerable<Location> GetServedLocations(string source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeatherAlert> GetAlerts(string source, Location location)
        {
            throw new NotImplementedException();
        }

        public void SetupPoller(string source, Location location)
        {
            throw new NotImplementedException();
        }

        public void TriggerAlertsNotification()
        {
            throw new NotImplementedException();
        }
    }

    public record Location();

    public record WeatherAlert(DateTime DateTime, int Severity, string Description);
}