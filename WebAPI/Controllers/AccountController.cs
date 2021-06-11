using Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Areas.Identity.Data;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IdentityContext _dbContext;
        private readonly UserManager<WebAPIUser> _userManager;
        private readonly SignInManager<WebAPIUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(IdentityContext dbContext,
            UserManager<WebAPIUser> userManager,
            SignInManager<WebAPIUser> signInManager,
            IConfiguration config)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> Token([FromBody] AccountLoginModel loginModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginModel.Email); //Skulle den inte logga in på username?
            if (user is not null)
            {
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
                        Expires = DateTime.UtcNow.AddDays(1), // TODO! Hur länge skulle den gälla?
                        SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Ok("Login failed, Signinresult was not successfull");
                }
            }
            else
            {
                return Ok("Login failed, please fill in a user name and password");
            }

            //return Ok("Login failed, did not enter the if-statement");
        }

        [AllowAnonymous]
        [HttpPost("api/account/register")]
        public async Task<ActionResult> Register([FromBody] AccountLoginModel loginModel)
        {
            var webApiSecuredUser = new WebAPIUser()
            {
                Email = loginModel.Email,
                UserName = loginModel.UserName,
                EmailConfirmed = true // TODO! Lägga till emailconfirmation?
            };

            var result = await _userManager.CreateAsync(webApiSecuredUser, loginModel.Password);

            if (result.Succeeded)
            {
                return Ok(new { Result = "Register success" });
            }
            else
            {
                StringBuilder errorString = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorString.Append(error.Description);
                }
                return Ok(new { Result = $"Registration failed: {errorString }" });
            }
        }
}
    }
