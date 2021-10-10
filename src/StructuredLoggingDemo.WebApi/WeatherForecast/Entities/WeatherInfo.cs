using System;

namespace StructuredLoggingDemo.WebApi.WeatherForecast.Entities
{
    public class WeatherInfo
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public override string ToString() => $"{Date}: {TemperatureC}°C / {TemperatureF}°F, {Summary}";
    }
}
