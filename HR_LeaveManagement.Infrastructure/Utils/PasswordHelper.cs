using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Utils
{
    public class PasswordHelper : IPasswordHelper
    {
        public byte[] GenerateSalt()
        {
            using var hmac = new HMACSHA256();
            byte[] salt = hmac.Key;
            return salt;
        }

        public string GenerateHashPassword(string password, byte[] salt)
        {
            using var hmac = new HMACSHA256(salt);
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashPassword = Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
            return hashPassword;
        }

        public bool VerifyHashPassword(string password, string hashPassword)
        {
            try
            {
                var splitHashPassword = hashPassword.Split(".");
                var salt = Convert.FromBase64String(splitHashPassword[0]);
                var storedPasswordHash = Convert.FromBase64String(splitHashPassword[1]);

                using var hmac = new HMACSHA256(salt);
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                Console.WriteLine("computeHash line 33: " + Convert.ToBase64String(computeHash));
                return CryptographicOperations.FixedTimeEquals(computeHash, storedPasswordHash);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
