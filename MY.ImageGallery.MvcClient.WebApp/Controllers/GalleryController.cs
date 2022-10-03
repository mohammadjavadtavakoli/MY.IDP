using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MY.ImageGallery.MvcClient.WebApp.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;

        public GalleryController(ILogger<GalleryController> logger )
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await WriteOutIdentityInformation();
            return null;
        }

        // GET: Gallery
        public async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            _logger.LogWarning($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                _logger.LogWarning($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

    }
}