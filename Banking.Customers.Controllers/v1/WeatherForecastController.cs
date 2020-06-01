using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Banking.Customers.Domain;
using Banking.Customers.Controllers.Attributes;
using Banking.Customers.Domain.Models;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Controllers.Managers;
using System.Threading.Tasks;

namespace Banking.Customers.Controllers.v1
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGreeting _saludos;
        private readonly IGreeting _greetings;
        private readonly IWeatherManager _weatherManager;


        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IGreeting saludos, IGreeting<EnglishGreetings> greeting, IWeatherManager weatherManager)
        {
            _logger = logger;
            _saludos = saludos;
            _greetings = greeting;
            _weatherManager = weatherManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("Test")]
        [ModelValidation]
        public IActionResult Post([FromBody] CustomerModel customer)
        {
            //if (!ModelState.IsValid)
                //return BadRequest(ModelState);
            return Ok(customer);
        }

        [HttpGet("Saludos")]
        public IActionResult GetSaludos()
        {
            return Ok(_saludos.GetGreetings());
        }

        [HttpGet("Greetings")]
        public IActionResult GetGreetings()
        {
            return Ok(_greetings.GetGreetings());
        }
        [HttpGet("Report")]
        public async Task<IActionResult> GetReport()
        {
            var report = await _weatherManager.GetWeatherForecastAsync("London");


            return Ok(report);
        }

    }
}
