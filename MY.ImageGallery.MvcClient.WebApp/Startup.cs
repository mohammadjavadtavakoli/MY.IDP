using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;
using Microsoft.IdentityModel.Tokens;
using MY.ImageGallery.MvcClient.Services;

namespace MY.ImageGallery.MvcClient.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllersWithViews();
            services.AddAuthentication(option =>
                {
                    option.DefaultScheme = "Cookies";
                    option.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", option => { option.AccessDeniedPath = "/Account/AccessDenied"; })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "https://localhost:6001";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "imagegalleryclient";
                    options.ClientSecret = "secret";

                    options.ResponseType = "code id_token";


                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("address");
                    options.Scope.Add("roles");
                    options.Scope.Add("country");
                    options.Scope.Add("subscriptionlevel");

                    options.Scope.Add("imagegalleryapi.access");

                    //Because it is not in the Microsoft map 
                    options.ClaimActions.MapJsonKey(claimType: "role", jsonKey: "role"); // for having 2 or more roles
                    options.ClaimActions.MapJsonKey(claimType: "subscriptionlevel", jsonKey: "subscriptionlevel");
                    options.ClaimActions.MapJsonKey(claimType: "country", jsonKey: "country");


                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.SignedOutCallbackPath = "/signout-callback-oidc";

                    options.ClaimActions.Remove("amr");
                    options.ClaimActions.DeleteClaim("sid");
                    options.ClaimActions.DeleteClaim("idp");

                    // It specifies how to validate the token received from IDP
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.GivenName,
                        RoleClaimType = JwtClaimTypes.Role,
                    };


                    // options.ClaimActions.DeleteClaim("address");
                });
            services.AddAuthorization(option =>
            {
                option.AddPolicy(name: "CanOrderFrame", configurePolicy: policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireClaim(claimType: "country", allowedValues: "ir");
                    policyBuilder.RequireClaim(claimType: "subscriptionlevel", allowedValues: "payingUser");
                });
            });
            services.AddHttpContextAccessor();
            services.AddHttpClient<IImageGalleryHttpClient, ImageGalleryHttpClient>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}