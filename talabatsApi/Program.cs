
using Microsoft.EntityFrameworkCore;

using talabatRepository.Data;

using talabatsApi.middleware;
using talabatsApi.ExtensionMethod;

namespace talabatsApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create builder
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Register Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext
            builder.Services.AddDbContext<TalabatDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register custom services (extension method)
            builder.Services.AddServicesExtension();

            // Build the app
            var app = builder.Build();

            // Migrate and seed database after app is built
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                await serviceProvider.MigrateAndSeedAsync();
            }

            // Middleware configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            // Run the app
             app.Run();
        }
    }
}