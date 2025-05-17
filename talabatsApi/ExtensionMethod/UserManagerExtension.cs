using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.core.model.identity;

namespace TalabatApi.ExtensionMethods
{
    public static class UserManagerExtensions
    {
        public static async Task<appUser?> GetUserWithAddressAsync(this UserManager<appUser> userManager, ClaimsPrincipal user)
        {
            // Extract email from claims
            var email = user.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return null;

            // Query user including Address navigation property
            return await userManager.Users
                .Include(u => u.Address)
                .SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}