using System.Reflection;
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
            // services.AddSwaggerGen();

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IImageService, ImageService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                        .Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data")),
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    });
            });
            services.AddMvc();

            services.AddAutoMapper(typeof(ImageMappingsProfile).GetTypeInfo().Assembly);

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
        
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                // app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            
        }
    }
}

