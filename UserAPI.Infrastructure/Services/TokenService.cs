using DemoApp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApp.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Role Based Token Generation
        //public string GenerateToken(
        //    IdentityUser user,
        //    IList<string> roles)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(
        //            ClaimTypes.NameIdentifier,
        //            user.Id),

        //        new Claim(
        //            ClaimTypes.Name,
        //            user.UserName!)
        //    };

        //    foreach (var role in roles)
        //    {
        //        claims.Add(
        //            new Claim(
        //                ClaimTypes.Role,
        //                role));
        //    }

        //    var key =
        //        new SymmetricSecurityKey(
        //            Encoding.UTF8.GetBytes(
        //                _configuration["Jwt:Key"]!));

        //    var credentials =
        //        new SigningCredentials(
        //            key,
        //            SecurityAlgorithms.HmacSha256);

        //    var token =
        //        new JwtSecurityToken(
        //            issuer:
        //                _configuration["Jwt:Issuer"],
        //            audience:
        //                _configuration["Jwt:Audience"],
        //            claims: claims,
        //            expires:
        //                DateTime.UtcNow.AddHours(1),
        //            signingCredentials:
        //                credentials);

        //    return new JwtSecurityTokenHandler()
        //        .WriteToken(token);
        //}
        public string GenerateToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}