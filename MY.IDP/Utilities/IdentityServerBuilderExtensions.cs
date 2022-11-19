using IdentityServer4.Test;
using MY.IDP.Services;

namespace MY.IDP.Utilities
{
    public static class IdentityServerBuilderExtensions
    {
        // public static IIdentityServerBuilder AddTestUsers(this IIdentityServerBuilder builder, List<TestUser> users)
        // {
        //     builder.Services.AddSingleton(new TestUserStore(users));
        //     builder.AddProfileService<TestUserProfileService>();
        //     builder.AddResourceOwnerValidator<TestUserResourceOwnerPasswordValidator>();
        //
        //     return builder;
        // }
        
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            // builder.Services.AddScoped<IUsersService, UsersService>();
            builder.AddProfileService<CustomUserProfileService>();
            return builder;
        }
    }
}

