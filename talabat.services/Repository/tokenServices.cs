using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model.identity;
using Talabat.core.Services;

namespace talabat.services.Repository
{
    public class tokenServices : ItokenServices
    {
       public tokenServices(IConfiguration configuration)
        {
            Configuration = configuration;
            // Constructor logic if needed
        }

        public IConfiguration Configuration { get; }

        public async Task<string> CreateToken(appUser username)
        {
            // Implement payload creation logic here
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, username.DisplayName),
                new Claim(ClaimTypes.Email, username.Email)
            };
            // Add more claims as needed
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:KEY"]));
            var token = new JwtSecurityToken(
                issuer: Configuration["jwt:Issuer"],
                audience: Configuration["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);
            return await Task.FromResult(tokenString);


        }
    }
}
