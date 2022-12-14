using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MY.ImageGallery.MvcClient.Services;
using MY.ImageGallery.MvcClient.ViewModels;
using MY.WebApi.ImageGallery.Models;
using Newtonsoft.Json;
using NuGet.Common;

namespace MY.ImageGallery.MvcClient.WebApp.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IImageGalleryHttpClient _imageGalleryHttp;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GalleryController(IImageGalleryHttpClient imageGalleryHttp, IConfiguration configuration,
            HttpClient httpClient)
        {
            _imageGalleryHttp = imageGalleryHttp;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [Authorize(Policy = "CanOrderFrame")]
        public async Task<IActionResult> OrderFrame()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(_configuration["IDPBaseAddress"]);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await _httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken
            });

            if (response.IsError)
            {
                throw new Exception("problem accessing the user info", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(x => x.Type == "address")?.Value;

            return View(new OrderFrameViewModel(address));
        }

        [Authorize]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var result = this.User.Claims;

            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            foreach (var item in User.Claims)
            {
            }


            var httpClient = await _imageGalleryHttp.GetHttpClientAsync();
            var response = await httpClient.GetAsync("api/image");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            response.EnsureSuccessStatusCode();

            var imagesAsString = await response.Content.ReadAsStringAsync();
            var galleryIndexViewModel = new GalleryIndexViewModel(
                JsonConvert.DeserializeObject<IList<ImageModel>>(imagesAsString).ToList());
            return View(galleryIndexViewModel);
        }


        [Route("welcome")]
        public async Task<IActionResult> welcome()
        {
            return View("welcome");
        }

        public async Task<IActionResult> EditImage(Guid id)
        {
            var httpclient = await _imageGalleryHttp.GetHttpClientAsync();
            var response = await httpclient.GetAsync($"api/image/{id}");
            response.EnsureSuccessStatusCode();

            var imageAsString = await response.Content.ReadAsStringAsync();
            var deserializedImage = JsonConvert.DeserializeObject<ImageModel>(imageAsString);
            var editImageViewModel = new EditImageViewModel
            {
                Id = deserializedImage.Id,
                Title = deserializedImage.Title
            };
            return View(editImageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImage(EditImageViewModel editImageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var imageForUpdate = new ImageForUpdateModel { Title = editImageViewModel.Title };
            var serializedImageForUpdate = JsonConvert.SerializeObject(imageForUpdate);
            var httpclient = await _imageGalleryHttp.GetHttpClientAsync();
            var response = await httpclient.PutAsync(
                $"api/image/{editImageViewModel.Id}",
                new StringContent(serializedImageForUpdate, System.Text.Encoding.Unicode, "application/json"));
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var httpclient = await _imageGalleryHttp.GetHttpClientAsync();
            var response = await httpclient.DeleteAsync($"api/image/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PayingUser")]
        public async Task<IActionResult> AddImage(AddImageViewModel addImageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var imageForCreation = new ImageForCreationModel { Title = addImageViewModel.Title };
            var imageFile = addImageViewModel.Files.First();
            if (imageFile.Length > 0)
            {
                using (var fileStream = imageFile.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    imageForCreation.Bytes = ms.ToArray();
                }
            }

            var serializedImageForCreation = JsonConvert.SerializeObject(imageForCreation);
            var httplient = await _imageGalleryHttp.GetHttpClientAsync();
            var response = await httplient.PostAsync(
                $"api/image",
                new StringContent(serializedImageForCreation, System.Text.Encoding.Unicode, "application/json"));
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        public async Task Logout()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = "https://localhost:5076/welcome"
            };
             await RevokeTokens();
             
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("oidc", prop);
        }

        private async Task RevokeTokens()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(_configuration["IDPBaseAddress"]);

            var access_token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(access_token))
            {
                var response = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest()
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = _configuration["clientId"],
                    ClientSecret = _configuration["ClientSecret"],
                    Token = access_token
                });

                if (response.IsError)
                {
                    throw new Exception("Problem accessing the TokenRevocation endpoint.", response.Exception);
                }
            }

            var refresh_token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refresh_token))
            {
                var response = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest()
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = _configuration["clientId"],
                    ClientSecret = _configuration["ClientSecret"],
                    Token = refresh_token
                });

                if (response.IsError)
                {
                    throw new Exception("Problem accessing the TokenRevocation endpoint.", response.Exception);
                }
            }
        }
    }
}