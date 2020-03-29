using System;
using System.Threading.Tasks;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Controllers.Managers
{

    public interface IWeatherManager
    {
        Task<WeatherResponse> GetWeatherForecastAsync(string location);
    }


    public class WeatherManager: IWeatherManager
    {
        private WeatherResponse WeatherReport;

        private readonly IWeatherForecastService _service;

        public WeatherManager(IWeatherForecastService service)
        {
            _service = service;

        }

        public async Task<WeatherResponse> GetWeatherForecastAsync(string location)
        {
            if (WeatherReport != null) return WeatherReport;

            WeatherReport = await _service.GetWeatherForecastAsync(location);
            return WeatherReport;


        }
    }

    
}
