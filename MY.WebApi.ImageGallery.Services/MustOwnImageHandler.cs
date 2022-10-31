using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MY.WebApo.ImageGallery.Service
{
    public class MustOwnImageRequirement : IAuthorizationRequirement
    {
    }

    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        private readonly IImageService _imageService;
        private readonly ILogger<MustOwnImageHandler> _logger;

        public MustOwnImageHandler(IImageService imageService, ILogger<MustOwnImageHandler> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MustOwnImageRequirement requirement)
        {
            var filterContext = context.Resource as HttpContext;
            if (filterContext == null)
            {
                context.Fail();
                return;
            }
            
            var routeData = filterContext.GetRouteData();
            var imageid = routeData.Values["id"].ToString();

            if (!Guid.TryParse(imageid, out Guid imageIdAsGuid))
            {
                _logger.LogError($"{imageid} is not GUID");
                context.Fail();
                return;
            }

            var subClaim = context.User.Claims.FirstOrDefault(x => x.Type == "sub");
            if (subClaim == null)
            {
                _logger.LogError($"User.Claims don't have the `sub` claim.");
                context.Fail();
                return;
            }

            var ownerId = subClaim.Value;
            if (!await (_imageService.IsImageOwnerAsyn(imageIdAsGuid, ownerId)))
            {
                _logger.LogError($"`{ownerId}` is not the owner of `{imageIdAsGuid}` image.");
            }

            context.Succeed(requirement);
        }
    }
}