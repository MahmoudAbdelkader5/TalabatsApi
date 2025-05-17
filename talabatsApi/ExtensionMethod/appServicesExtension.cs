using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talabat.core.Repository;
using talabatRepository.Data;
using talabatRepository.Identity;
using talabatRepository.Repository;
using TalabatRepository.Repository;
using talabatsApi.Error;
using talabatsApi.Helper;

namespace talabatsApi.ExtensionMethod
{
    public static class AppServicesExtension
    {
        public static IServiceCollection AddServicesExtension(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(IgerenicRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IBasketRepo, BasketRepo>();

            services.AddAutoMapper(typeof(profiles));

            // Configure API behavior
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(p => p.Value.Errors.Count > 0)
                        .SelectMany(p => p.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var validationError = new ApiValidationError
                    {
                        errors = errors
                    };
                    return new BadRequestObjectResult(validationError);
                };
            });

            return services;
        }

        public static async Task MigrateAndSeedAsync(this IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = serviceProvider.GetRequiredService<TalabatDbContext>();
                var identityContext = serviceProvider.GetRequiredService<addIdentityDbcontext>();
               await identityContext.Database.MigrateAsync();

                await context.Database.MigrateAsync();

                await dbcontextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("MigrationLogger");
                logger.LogError(ex, "An error occurred during migration");
            }
        }
    }
}