using Microsoft.EntityFrameworkCore;
using MY.IDP.DataLayer.Context;
using MY.IDP.Services;
using MY.IDP.Settings;

namespace MY.IDP
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
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(Config.GetUsers())
                .AddInMemoryIdentityResources(Config.GetIdentityResource())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            InitializeDb(app);

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.Run();
        }

        private static void InitializeDb(IApplicationBuilder app)
        {
            //service locator pattern
            
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope=scopeFactory.CreateScope())
            {
                using (var context=scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context?.Database.Migrate();
                }
            }
        }
    }
}