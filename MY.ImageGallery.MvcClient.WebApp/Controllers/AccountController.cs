using Microsoft.AspNetCore.Mvc;

namespace MY.ImageGallery.MvcClient.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

