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
using HR_LeaveManagement.Application.DTOs;
using Microsoft.Extensions.Logging;


namespace HR_LeaveManagement.Infrastructure.Utils
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtService> _logger;

        public JwtService(
            IOptions<JwtSettings> jwtSettings, 
            ILogger<JwtService> logger
            )
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
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

        public JwtValidationResultDto ValidateAndExtractToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var validatiionParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true, // Let framework handle expiry
                ClockSkew = TimeSpan.Zero,

                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(jwtToken, validatiionParameters, out _);
            _logger.LogInformation($"principals = {principal}");

            _logger.LogInformation("Read all claims");
            string GetClaim(string name) =>
                principal.FindFirst(name)?.Value
                ?? throw new SecurityTokenException($"Missing claim: {name}");

            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
            var email = GetClaim("email");
            var userId = GetClaim("sub");
            var roles = GetClaim("role");
            var permissions = GetClaim("permission");

            return new JwtValidationResultDto(
                email, 
                userId,
                roles,
                permissions
            );
        }
    }
}
