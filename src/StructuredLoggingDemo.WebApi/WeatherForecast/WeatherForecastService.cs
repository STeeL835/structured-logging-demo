using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using StructuredLoggingDemo.WebApi.WeatherForecast.Entities;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    public interface IWeatherForecastService
    {
        WeatherInfo GetForecast(DateTime date);
        IReadOnlyList<WeatherInfo> GetForecast(DateTime dateFrom, DateTime dateTo);
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly ILogger<WeatherForecastService> _logger;

        private readonly Random _random = new Random();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }

        public WeatherInfo GetForecast(DateTime date)
        {
            _logger.LogDebug("Getting forecast for {ForecastDate}", date);

            return new WeatherInfo()
            {
                Date = date.Date,
                TemperatureC = _random.Next(-20, 55),
                Summary = Summaries[_random.Next(Summaries.Length)]
            };
        }

        public IReadOnlyList<WeatherInfo> GetForecast(DateTime dateFrom, DateTime dateTo)
        {
            var date = dateFrom;
            var weatherInfos = new List<WeatherInfo>();

            while (date <= dateTo)
            {
                weatherInfos.Add(GetForecast(date));
                date = date.AddDays(1);
            }

            return weatherInfos;
        }
    }
}