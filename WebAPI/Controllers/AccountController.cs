using Entities;
using Entities.Models;
using Identity.DataTransferObjects;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
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
        private readonly ILoggerManager _logger;

        public AccountController(IdentityContext idDbContext,
            PropertyContext propertyContext,
            UserManager<WebAPIUser> userManager,
            SignInManager<WebAPIUser> signInManager,
            IConfiguration config,
            ILoggerManager logger)
        {
            _idDbContext = idDbContext;
            _propertyContext = propertyContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Tries to log in the user and returns a token.
        /// This token is nestled into a JsonObject.
        /// </summary>
        /// 
        /// <param name="loginModel"></param>
        /// <param name="grant_type"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> Token([FromForm] AccountLoginDto loginModel, string grant_type)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AccountLoginDto sent from client is null");
                return UnprocessableEntity(ModelState);
            }
            var user = _idDbContext.Users.FirstOrDefault(x => x.Email == loginModel.Username);
            if (user is null)
            {
                return Ok("Login failed, please fill in a registered Email address");
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!signInResult.Succeeded)
            {
                return Ok("Login failed, Signinresult was not successfull");
            }
            var tokenGiver = new TokenObjectHelper(_config);
            var tokenObject = tokenGiver.TokenHelper(loginModel.Username);
            var jsonToken = JsonConvert.SerializeObject(tokenObject);

            return Ok(jsonToken);
        }
        /// <summary>
        /// Tries to register a user - first in the
        /// identityDb and then in the propertyDb
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/account/register")]
        public async Task<ActionResult> Register([FromForm] AccountRegisterDto registerModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AccountRegisterDto sent from client is null");
                return UnprocessableEntity(ModelState);
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