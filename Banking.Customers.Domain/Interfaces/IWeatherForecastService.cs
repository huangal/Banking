using System.Threading.Tasks;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Domain.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<WeatherResponse> GetWeatherForecastAsync(string location);
    }
}
