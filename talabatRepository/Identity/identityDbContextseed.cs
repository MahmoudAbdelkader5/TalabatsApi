using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Talabat.core.model.identity;

namespace talabatRepository.Identity
{
    public static class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<appUser> userManager, ILogger logger = null)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var user = new appUser
                    {
                        DisplayName = "Mahmoud Abdelkader",
                        Email = "mm4084@fayoum.edu.eg",
                        UserName = "mm4084@fayoum.edu.eg", // Best practice: use email as username
                        PhoneNumber = "01234567891",
                        EmailConfirmed = true // Important for immediate access
                    };

                    // Create a strong password (in production, this would come from secure config)
                    var password = "P@ssw0rd123"; // Example strong password

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception($"User creation failed: {errors}");
                    }

                    // Optionally add roles here if needed
                    // await userManager.AddToRoleAsync(user, "Admin");

                    logger?.LogInformation("Default user created successfully");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred while seeding the identity database");
                throw; // Re-throw to ensure the startup fails if seeding fails
            }
        }
    }
}