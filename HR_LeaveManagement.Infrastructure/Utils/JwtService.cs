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


namespace HR_LeaveManagement.Infrastructure.Utils
{
    public class JwtService : IJwtService
    {
        public JwtService()
        {
            
        }
        public string[] GetSecretKeys()
        {
            string ACCESS_TOKEN_SECRET = "";
            string REFRESH_TOKEN_SECRET = "";
            return [ACCESS_TOKEN_SECRET, REFRESH_TOKEN_SECRET];
        }

        public string GenerateAccessToken(User user, string keyValue, string issuerValue, string audienceValue)
        {
            var key = Encoding.UTF8.GetBytes(keyValue);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(user.Id)),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Roles),
            };

            var token = new JwtSecurityToken(
                issuer: issuerValue,
                audience: audienceValue,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
