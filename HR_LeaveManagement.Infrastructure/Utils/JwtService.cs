using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using HR_LeaveManagement.Application.Models;


namespace HR_LeaveManagement.Infrastructure.Utils
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string[] GetSecretKeys()
        {
            string ACCESS_TOKEN_SECRET = "";
            string REFRESH_TOKEN_SECRET = "";
            return [ACCESS_TOKEN_SECRET, REFRESH_TOKEN_SECRET];
        }

        public string GenerateAccessToken(User user, IList<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            string result = Convert.ToBase64String(randomNumber).ToString();
            return result;
        }
    }
}
