using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Role
{
    public class AssignRoleToUserDto
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}
