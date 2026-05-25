using HR_LeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Domain.Entities
{
    public class UserRole 
    {
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
