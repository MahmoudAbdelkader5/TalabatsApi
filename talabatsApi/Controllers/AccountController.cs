using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.core.model.identity;
using Talabat.core.Services;
using TalabatApi.ExtensionMethods;
using talabatsApi.DTO;
using talabatsApi.Error;

namespace talabatsApi.Controllers
{

    public class AccountController : ApiBaseController
    {
        private readonly SignInManager<appUser> signInManager;
        private readonly ItokenServices tokenservices;

        public AccountController(UserManager<appUser> userManager, SignInManager<appUser> signInManager, ItokenServices tokenservices)
        {
            UserManager = userManager;
            this.signInManager = signInManager;
            this.tokenservices = tokenservices;
        }

        public UserManager<appUser> UserManager { get; }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email already exists"));
            }
            var user = new appUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await UserManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult<ActionResult<UserDto>>(new UserDto
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await tokenservices.CreateToken(user) // Replace with actual token generation logic
                });
            }
            else
            {
                return BadRequest(new ApiResponse(404));
            }


        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                return await Task.FromResult<ActionResult<UserDto>>(new UserDto
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await tokenservices.CreateToken(user)
                });
            }
            else
            {
                return Unauthorized(new ApiResponse(401));
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
         var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            return await Task.FromResult<ActionResult<UserDto>>(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await tokenservices.CreateToken(user)
            });

        }
        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await UserManager.GetUserWithAddressAsync(User);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
           var mappedAddress = new AddressDto
           {
               FirstName = user.Address.FirstName,
               LastName = user.Address.LastName,
               Street = user.Address.Street,
               City = user.Address.City,
               
           };
            return Ok(mappedAddress);

        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {

            var user = await UserManager.GetUserWithAddressAsync(User);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            user.Address.appUserId = user.Id;
            user.Address.FirstName = addressDto.FirstName;
            user.Address.LastName = addressDto.LastName;
            user.Address.Street = addressDto.Street;
            user.Address.City = addressDto.City;
            user.Address.Country = addressDto.Country;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return await Task.FromResult<ActionResult<AddressDto>>(new AddressDto
                {
                    FirstName = user.Address.FirstName,
                    LastName = user.Address.LastName,
                    Street = user.Address.Street,
                    City = user.Address.City,
                    Country = user.Address.Country
                });
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }

        }
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await UserManager.Users.FirstOrDefaultAsync(UserManager => UserManager.Email == email) != null;
        }



    }
}
