using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Utils
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            return Random.Shared.Next(111111, 999999).ToString();
        }
    }
}
