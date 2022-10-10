using Microsoft.AspNetCore.Authentication.Cookies;
using MY.ImageGallery.MvcClient.Services;

namespace MY.ImageGallery.MvcClient.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "https://localhost:6001";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "imagegalleryclient";
                    options.ClientSecret = "secret";

                    options.ResponseType = "code";
                    
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.SignedOutCallbackPath = "/signout-callback-oidc";



                });
            services.AddHttpContextAccessor();
            services.AddHttpClient<IImageGalleryHttpClient,ImageGalleryHttpClient>();


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

