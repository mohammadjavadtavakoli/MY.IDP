using Microsoft.AspNetCore.Authentication.Cookies;
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
            services.AddControllersWithViews();
            services.AddAuthentication(option =>
                {
                    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:6001";
                    options.ClientId = "imagegalleryclient";
                    options.Resource = "code code id_token";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.SaveTokens = true;
                    options.ClientSecret = "secret";
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

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

