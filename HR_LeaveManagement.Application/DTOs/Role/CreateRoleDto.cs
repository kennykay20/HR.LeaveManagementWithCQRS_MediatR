using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Role
{
    public class CreateRoleDto : IRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
