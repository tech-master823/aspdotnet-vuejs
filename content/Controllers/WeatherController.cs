using System.Linq;
using System.Threading;
using AspDotnetVueJS.Providers;
using Microsoft.AspNetCore.Mvc;

namespace AspDotnetVueJS.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherProvider _weatherProvider;

        public WeatherController(IWeatherProvider weatherProvider)
        {
            this._weatherProvider = weatherProvider;
        }

        [HttpGet("[action]")]
        public IActionResult Forecasts([FromQuery(Name = "from")] int from = 0, [FromQuery(Name = "to")] int to = 4)
        {
            var quantity = to - from;

            // We should also avoid going too far in the list.
            if (quantity <= 0) return BadRequest("You cannot have the 'to' parameter higher than 'from' parameter.");

            if (from < 0) return BadRequest("You cannot go in the negative with the 'from' parameter");

            var allForecasts = _weatherProvider.GetForecasts();
            var result = new
            {
                Total = allForecasts.Count,
                Forecasts = allForecasts.Skip(from).Take(quantity).ToArray()
            };
            
            Thread.Sleep(2000);

            return Ok(result);
        }
    }
}
