using HR_LeaveManagement.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.DTOs.Permission
{
    public class PermissionDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
