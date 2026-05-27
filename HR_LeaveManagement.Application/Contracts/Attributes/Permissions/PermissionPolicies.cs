using HR_LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Attributes.Permissions
{
    public static class PermissionPolicies
    {
        public static void AddPermissionPolicies(AuthorizationOptions options)
        {
            var subPermissions = new PermissionSubPolicies();
            //var permissions = new List<string>();
            var permissions = subPermissions.GetLeavePermissions();

            foreach (var permission in permissions)
            {
                options.AddPolicy(permission,
                    policy => policy.Requirements.Add(
                        new PermissionRequirement(permission)));
            }
        }
    }

    public class PermissionSubPolicies
    {
        public List<string> GetLeavePermissions()
        {
            const string Create = "leave.create";
            const string Update = "leave.update";
            const string Delete = "leave.delete";
            const string Approve = "leave.approve";

            string[] SubPermissions = [Create, Update, Delete, Approve];

            return SubPermissions.ToList();
        }
    }
}
