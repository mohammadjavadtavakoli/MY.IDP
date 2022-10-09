using System.Net.Http.Headers;

namespace MY.ImageGallery.MvcClient.Services;

public interface IImageGalleryHttpClient
{
    HttpClient HttpClient { get; }
}

public class ImageGalleryHttpClient : IImageGalleryHttpClient
{
    
    public HttpClient HttpClient { get; }

    public ImageGalleryHttpClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("http://localhost:5065/");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpClient = httpClient;
    }
}