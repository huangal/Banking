using System.IO;
using System.Net.Http;

namespace Banking.Customers.Services
{

    public interface IImageService
    {
        string GetImageAsync(string imageUrl);
        Stream GetImage(string imageUrl);
    }

    public class ImageService : IImageService
    {
        private readonly IHttpClientFactory ClientFactory;
        public ImageService(IHttpClientFactory httpClientFactory)
        {
            ClientFactory = httpClientFactory;
        }

        public  string GetImageAsync(string imageUrl)
        {
            var client = ClientFactory.CreateClient();

            //var handler = new HttpClientHandler();
            //handler.ClientCertificates.Add(certificate);



            var response =  client.GetAsync(imageUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var pageContents = response.Content.ReadAsStringAsync();
                return pageContents.Result;
            }

            return string.Empty;
        }

        public Stream GetImage(string imageUrl)
        {
            var client = ClientFactory.CreateClient();
            var response = client.GetAsync(imageUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var pageContents = response.Content.ReadAsStreamAsync();
                return pageContents.Result;
            }

            return null;

        }


    }
}
