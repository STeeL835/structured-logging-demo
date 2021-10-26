using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StructuredLoggingDemo.WebApi.Emailing;
using StructuredLoggingDemo.WebApi.WeatherForecast.Entities;

namespace StructuredLoggingDemo.WebApi.WeatherForecast
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _forecastService;
        private readonly IEmailService _emailService;
        private readonly IWeatherAlertsService _weatherAlertsService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            IWeatherForecastService forecastService, 
            IEmailService emailService, 
            IWeatherAlertsService weatherAlertsService)
        {
            _logger = logger;
            _forecastService = forecastService;
            _emailService = emailService;
            _weatherAlertsService = weatherAlertsService;
        }


        [HttpGet("week")]
        public ActionResult<IEnumerable<WeatherInfo>> GetWeekForecast()
        {
            var today = DateTime.Today;
            return Ok(_forecastService.GetForecast(today, today.AddDays(7)));
        }

        [HttpPost("week/email")]
        public ActionResult GetWeekForecastByEmail([FromQuery] string emailAddress)
        {
            var today = DateTime.Today;
            var forecast =  _forecastService.GetForecast(today, today.AddDays(7));

            _logger.LogInformation("Sending email");
            _emailService.SendEmail(emailAddress, string.Join('\n', forecast));

            return Ok();
        }


        [HttpPut("upload/alerts")]
        public ActionResult<IDictionary<string, AlertsUpdateResult>> UpdateAlerts([FromBody] List<string> sources) // TODO: observable process
        {
            return Ok(_weatherAlertsService.UpdateAlerts(sources));
        }
    }
}
