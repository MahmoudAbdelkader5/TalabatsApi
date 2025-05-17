using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text;
using talabatRepository.Data;
using talabatsApi.middleware;
using talabatsApi.ExtensionMethod;
using talabatRepository.Identity;
using Talabat.core.model.identity;
using Talabat.core.Services;
using talabat.services.Repository;

namespace talabatsApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database Context for main data
            builder.Services.AddDbContext<TalabatDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Identity DbContext
            builder.Services.AddDbContext<addIdentityDbcontext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultIdentityConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Register Token Services
            builder.Services.AddScoped<ItokenServices, tokenServices>();

            // Configure Identity
            builder.Services.AddIdentity<appUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<addIdentityDbcontext>()
            .AddDefaultTokenProviders();

            // Redis connection
            builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    EndPoints = { builder.Configuration.GetConnectionString("Redis") },
                    AbortOnConnectFail = false,
                    ConnectRetry = 5,
                    ConnectTimeout = 5000
                }));

            // Add custom extension services
            builder.Services.AddServicesExtension();

            // Configure Authentication with JWT Bearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["jwt:KEY"])),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Token:Audience"],
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            // CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Migrate and seed databases
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<TalabatDbContext>();
                    await context.Database.MigrateAsync();
                    await dbcontextSeed.SeedAsync(context);

                    var identityContext = services.GetRequiredService<addIdentityDbcontext>();
                    await identityContext.Database.MigrateAsync();

                    var userManager = services.GetRequiredService<UserManager<appUser>>();
                    await IdentityDbContextSeed.SeedAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during database migration or seeding");
                }
            }

            // Middleware pipeline
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Handle status code pages
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // CORS before authentication
            app.UseCors("AllowAll");

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            await app.RunAsync();
        }
    }
}