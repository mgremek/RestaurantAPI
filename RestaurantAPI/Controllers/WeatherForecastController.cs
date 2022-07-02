using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherForecastService _forecastService;
       
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpPost]
        [Route("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Get([FromQuery]int returnedResultNumber, [FromBody]TempRange tempValueRange)
        {
            if (returnedResultNumber <= 0 || tempValueRange.MinValue >= tempValueRange.MaxValue)
                return BadRequest();

            return Ok(_forecastService.Get(returnedResultNumber, tempValueRange.MinValue, tempValueRange.MaxValue));
        }       
    }
}
