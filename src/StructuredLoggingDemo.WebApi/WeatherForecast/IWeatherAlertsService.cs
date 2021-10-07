using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    public interface IWeatherAlertsService
    {
        IReadOnlyList<AlertsUpdateResult> UpdateAlerts(List<string> sources);
    }

    public class WeatherAlertsService : IWeatherAlertsService
    {
        private readonly ILogger<WeatherAlertsService> _logger;
        private readonly IWeatherAlertsHelper _alertsHelper;
        private readonly IWeatherAlertsRepository _alertsRepository;

        public WeatherAlertsService(ILogger<WeatherAlertsService> logger, IWeatherAlertsHelper alertsHelper, IWeatherAlertsRepository alertsRepository)
        {
            _logger = logger;
            _alertsHelper = alertsHelper;
            _alertsRepository = alertsRepository;
        }

        public IReadOnlyList<AlertsUpdateResult> UpdateAlerts(List<string> sources)
        {
            var alertsUpdateResults = new List<AlertsUpdateResult>();

            foreach (var source in sources)
            {
                using (_logger.BeginScope("Updating alerts from {AlertSource}", source))
                {
                    try
                    {
                        UpdateAlertsForSource(source);
                        alertsUpdateResults.Add(new AlertsUpdateResult(source, true, "Success"));
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, "Couldn't update alerts from source");
                        alertsUpdateResults.Add(new AlertsUpdateResult(source, false, "Failed"));
                    }
                    _logger.LogInformation("Finished processing source");
                }
            }

            _logger.LogInformation("Finished updating all alert information, triggering alerts");
            _alertsHelper.TriggerAlertsNotification();

            return alertsUpdateResults;
        }

        private void UpdateAlertsForSource(string source)
        {
            var locations = _alertsHelper.GetServedLocations(source); // TODO: spontaneous exception, like source is down, or returns some response v2

            foreach (var location in locations)
                UpdateAlertsForLocation(source, location);
        }

        private void UpdateAlertsForLocation(string source, Location location)
        {
            using var scope = _logger.BeginScope("Processing location '{AlertLocation}'", location);

            var alerts = _alertsHelper.GetAlerts(source, location);
            var alertInfo = _alertsRepository.GetByLocation(location);

            if (alertInfo is null)
            {
                _logger.LogInformation("Location doesn't exist, creating new", location);
                _alertsRepository.Add(new AlertWeatherInfo(location, alerts.ToList(), new List<string> { source }));

                _logger.LogInformation("Setting up pollers", location);
                _alertsHelper.SetupPoller(source, location);
            }
            else
            {
                _logger.LogInformation("Found location, updating sources", location);
                _alertsRepository.UpdateAlerts(alertInfo with { sources = alertInfo.sources.Append(source).ToList() });
            }
        }
    }

    public record AlertsUpdateResult(string Source, bool Ok, string Message);
}