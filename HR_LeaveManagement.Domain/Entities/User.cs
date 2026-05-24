using HR_LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Domain.Entities
{
    public class User : BaseCommonEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string Roles { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsNewUser { get; set; }
        public string Otp { get; set; } = string.Empty;
        public DateTime? OtpExpiry { get; set; }
        public string RegistrationToken { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
