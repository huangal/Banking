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
        private const string key = "19554e5b6f1aa1d1d8fad49535446ce2";

        public WeatherForecastService(HttpClient client)
        {
			_httpClient = client;
        }
       

		public async Task<WeatherResponse> GetWeatherForecastAsync(string location)
		{
            //string url = $@"http://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID=19554e5b6f1aa1d1d8fad49535446ce2";
            string url = $@"http://api.openweathermap.org/data/2.5/weather?q={location}&APPID={key}";
            //string url = $@"https://samples.openweathermap.org/data/2.5/weather?q={location},uk&appid=b6907d289e10d714a6e88b30761fae22";
           // string uri = $"https://samples.openweathermap.org/data/2.5/weather?q={location},uk&appid=b6907d289e10d714a6e88b30761fae22";

            using(var response = await _httpClient.GetAsync(url))
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