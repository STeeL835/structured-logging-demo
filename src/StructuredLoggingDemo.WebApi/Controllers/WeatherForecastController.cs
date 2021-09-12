using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StructuredLoggingDemo.WebApi.Services;

namespace StructuredLoggingDemo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _forecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => _forecastService.GetForecast(DateTime.Today.AddDays(index)))
            .ToArray();
        }
    }
}
