﻿using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.EntityFrameworkCore;
using MY.WebApi.ImageGallery.DataLayer.Context;
using MY.WebApi.ImageGallery.Mappings;
using MY.WebApo.ImageGallery.Service;

namespace MY.WebApi.ImageGallery.WebApp
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
            // services.AddControllers();
            // services.AddEndpointsApiExplorer();
             services.AddSwaggerGen();

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IImageService, ImageService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                  
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    });
            });
            services.AddAuthentication(defaultScheme: IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IDPBaseAddress"];
                    //check audience token(aud) equal to imagegalleryapi
                    options.ApiName = "imagegalleryapi";
                    

                });
            services.AddMvc();

            services.AddAutoMapper(typeof(ImageMappingsProfile).GetTypeInfo().Assembly);

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
        
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                 app.UseSwaggerUI();
            }
            
            InitializeDb(app);

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();


            app.MapControllers();
            

            app.Run();
            
        }

        private static void InitializeDb(IApplicationBuilder app)
        {
            //service locator pattern
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                using (var context=scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context?.Database.Migrate();
                }
            }
        }
    }
}

