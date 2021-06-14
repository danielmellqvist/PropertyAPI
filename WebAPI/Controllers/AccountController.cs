using Entities;
using Entities.Models;
using Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Areas.Identity.Data;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IdentityContext _idDbContext;
        private readonly PropertyContext _propertyContext;
        private readonly UserManager<WebAPIUser> _userManager;
        private readonly SignInManager<WebAPIUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(IdentityContext idDbContext,
            PropertyContext propertyContext,
            UserManager<WebAPIUser> userManager,
            SignInManager<WebAPIUser> signInManager,
            IConfiguration config)
        {
            _idDbContext = idDbContext;
            _propertyContext = propertyContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> Token([FromBody] AccountLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).ToList();
                return Ok(errors);
            }
            var user = _idDbContext.Users.FirstOrDefault(x => x.Email == loginModel.Email);
            if (user is null)
            {
                return Ok("Login failed, please fill in a registered Email adress");
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (signInResult.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("EncryptionKey"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginModel.Email)
                }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString }); // TODO! Token är inte helt klar!
            }
            else
            {
                return Ok("Login failed, Signinresult was not successfull");
            }
        }
        [AllowAnonymous]
        [HttpPost("api/account/register")]
        public async Task<ActionResult> Register([FromBody] AccountRegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).ToList();
                return Ok(errors);
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                return Ok("The confirm password does not match the password");
            }
            var webApiSecuredUser = new WebAPIUser()
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(webApiSecuredUser, registerModel.Password);

            if (result.Succeeded)
            {
                var newUser = new User
                {
                    UserName = webApiSecuredUser.UserName,
                    IdentityUserId = Guid.Parse(webApiSecuredUser.Id)
                };
                _propertyContext.Users.Add(newUser);
                _propertyContext.SaveChanges();
                return Ok(new { Result = "Register success" });
            }
            else
            {
                StringBuilder errorString = new();
                foreach (var error in result.Errors)
                {
                    errorString.Append(error.Description);
                }
                return Ok(new { Result = $"Registration failed: {errorString }" });
            }
        }
    }
}
