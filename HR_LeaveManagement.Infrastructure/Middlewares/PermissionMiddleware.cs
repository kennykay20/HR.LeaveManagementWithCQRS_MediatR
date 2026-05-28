using HR_LeaveManagement.Application.Contracts.Attributes.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context, 
            ILogger<PermissionMiddleware> logger)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var permissionAttributes = endpoint.Metadata
                .GetOrderedMetadata<PermissionAttribute>();

            if (permissionAttributes == null || !permissionAttributes.Any())
            {
                await _next(context);
                return;
            }

            var userPermissions = context.User.Claims
                .Where(x => x.Type == "permission")
                .Select(x => x.Value)
                .ToList();

            foreach (var permission in permissionAttributes)
            {
                if (!userPermissions.Contains(permission.Permission))
                {
                    logger.LogWarning(
                        "User lacks permission: {Permission}",
                        permission.Permission
                    );

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "Forbidden"
                    });

                    return;
                }
            }

            await _next(context);
        }
    }
}
