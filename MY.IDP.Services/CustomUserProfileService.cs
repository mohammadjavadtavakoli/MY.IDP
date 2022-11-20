using System.Security.Claims;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace MY.IDP.Services
{
    public class CustomUserProfileService:IProfileService
    {
        private readonly IUserService _userService;

        public CustomUserProfileService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = await _userService.GetUserClaimsBySubjectIdAsync(subjectId);
            context.IssuedClaims = claimsForUser.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = await _userService.IsUserActiveAsync(subjectId);
        }
    }
}

