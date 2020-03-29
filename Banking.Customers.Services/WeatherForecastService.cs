using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
		private HttpClient _httpClient;

        public WeatherForecastService(HttpClient client)
        {
			_httpClient = client;
        }
       

		public async Task<WeatherResponse> GetWeatherForecastAsync(string location)
		{
			var uri = "https://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22";

            using(var response = await _httpClient.GetAsync(uri))
            {
				if (response.IsSuccessStatusCode)
                {
				     var responseStream = await response.Content.ReadAsStreamAsync();
					var report = await JsonSerializer.DeserializeAsync<WeatherResponse>(responseStream);
                    return report;
				}
                else
                {
					return null;
                }
                
			}  
		}



	}

}