using System.Collections.Generic;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    public interface IWeatherAlertsRepository
    {
        AlertWeatherInfo GetByLocation(Location location);
        AlertWeatherInfo Add(AlertWeatherInfo location);
        AlertWeatherInfo UpdateAlerts(AlertWeatherInfo location);
    }

    public class WeatherAlertsRepository : IWeatherAlertsRepository
    {
        public AlertWeatherInfo GetByLocation(Location location)
        {
            throw new System.NotImplementedException();
        }

        public AlertWeatherInfo Add(AlertWeatherInfo location)
        {
            throw new System.NotImplementedException();
        }

        public AlertWeatherInfo UpdateAlerts(AlertWeatherInfo location)
        {
            throw new System.NotImplementedException();
        }
    }

    public record AlertWeatherInfo(Location Location, List<WeatherAlert> Alerts, List<string> sources);
}