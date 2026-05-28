using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Attributes.Permissions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : Attribute
    {
        public string Permission { get; }
        public PermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
