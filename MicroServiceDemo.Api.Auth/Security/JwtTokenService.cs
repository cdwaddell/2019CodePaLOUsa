using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Models;
using MicroServiceDemo.Api.Blog.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroServiceDemo.Api.Auth.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public JwtTokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public void PopulateToken(UserDto user)
        {
            var appSettings = _appSettings.Value;

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            }, "password");

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = appSettings.Issuer,
                Issuer = appSettings.Issuer,
                Subject = claimsIdentity,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);

            user.Token = tokenHandler.WriteToken(plainToken);
        }
    }
}
