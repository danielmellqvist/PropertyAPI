
using Identity.DataTransferObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Areas.Identity.Data
{
    public class TokenObjectHelper
    {
        private readonly IConfiguration _config;

        public TokenObjectHelper(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Creates the token for the user and returns it in the form of a TokenObject
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public TokenObjectDto TokenHelper(string userEmail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("EncryptionKey"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userEmail)
            }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var timeSpan = (DateTime)tokenDescriptor.Expires - (DateTime)tokenDescriptor.IssuedAt;
            var secondSpan = (int)timeSpan.TotalSeconds;

            var formattedIssued = (DateTime)tokenDescriptor.IssuedAt;
            var formatterdExpire = (DateTime)tokenDescriptor.Expires;

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenObject = new TokenObjectDto
            {
                AcessToken = tokenHandler.WriteToken(token),
                TokenType = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                ExpiresIn = secondSpan,
                UserName = userEmail,
                TokenIssued = formattedIssued.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘UTC’"),
                TokenExpires = formatterdExpire.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘UTC’")
            };
            return tokenObject;
        }
    }
}
