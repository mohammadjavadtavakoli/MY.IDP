using Microsoft.EntityFrameworkCore;
using MY.IDP.DataLayer;
using MY.IDP.DataLayer.Context;
using MY.IDP.Services;
using MY.IDP.Settings;
using MY.IDP.Utilities;

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
            services.AddScoped<IUnitOfWork, MyApplicationDbContext>();

            services.AddScoped<IUserService, UsersService>();
            
            services.AddDbContext<MyApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                        serverDbContextOptionsBuilder.MigrationsAssembly("MY.IDP.DataLayer");
                    });
            });
            
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                // .AddTestUsers(Config.GetUsers())
                .AddCustomUserStore()
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
                using (var context=scope.ServiceProvider.GetService<MyApplicationDbContext>())
                {
                    context?.Database.Migrate();
                }
            }
        }
    }
}