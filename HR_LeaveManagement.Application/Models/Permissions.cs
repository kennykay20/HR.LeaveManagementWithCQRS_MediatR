using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Models
{
    public static class Permissions
    {
        public static class Leave
        {
            public const string Create = "leave.create";
            public const string Update = "leave.update";
            public const string Delete = "leave.delete";
            public const string Approve = "leave.approve";
        }

        public static class Role
        {
            public const string Create = "role.create";
            public const string Update = "role.update";
            public const string Delete = "role.delete";
        }
    }
}
