using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MY.ImageGallery.MvcClient.Services
{
    
    public interface IImageGalleryHttpClient
    {
        Task<HttpClient> GetHttpClientAsync();
    }
    public class ImageGalleryHttpClient : IImageGalleryHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageGalleryHttpClient(
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var currentContext = _httpContextAccessor.HttpContext;
            var accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                _httpClient.SetBearerToken(accessToken);
            }
            _httpClient.BaseAddress = new Uri(_configuration["WebApiBaseAddress"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }
    }

