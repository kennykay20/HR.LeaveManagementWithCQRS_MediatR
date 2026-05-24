using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces
{
    public interface IPasswordHelper
    {
        byte[] GenerateSalt();

        string GenerateHashPassword(string password, byte[] salt);

        bool VerifyHashPassword(string password, string hashPassword);
    }
}
