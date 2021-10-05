using System;

namespace StructuredLoggingDemo.WebApi.Services
{
    public interface IWeatherForecastService
    {
        WeatherForecast GetForecast(DateTime date);
    }

    public class FakeWeatherForecastService : IWeatherForecastService
    {
        private readonly Random _random = new Random();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecast GetForecast(DateTime date)
        {
            return new WeatherForecast()
            {
                Date = date.Date,
                TemperatureC = _random.Next(-20, 55),
                Summary = Summaries[_random.Next(Summaries.Length)]
            };
        }
    }
}