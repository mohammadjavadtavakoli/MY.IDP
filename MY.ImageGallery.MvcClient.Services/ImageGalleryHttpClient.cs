using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace MY.ImageGallery.MvcClient.Services
{
    public interface IImageGalleryHttpClient
    {
        HttpClient HttpClient { get; }
    }

    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
    
        public HttpClient HttpClient { get; }

        public ImageGalleryHttpClient(HttpClient httpClient,IConfiguration configuration)
        {
            
            httpClient.BaseAddress = new Uri(configuration["WebApiBaseAddress"]);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient = httpClient;
        }
    }
}

