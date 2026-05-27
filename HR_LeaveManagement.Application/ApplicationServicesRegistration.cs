using Hangfire;
using HR_LeaveManagement.Application.Contracts.Attributes.Permissions;
using HR_LeaveManagement.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HR_LeaveManagement.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Leave.Create, policy => policy.Requirements.Add(new PermissionRequirement(Permissions.Leave.Create)));

                options.AddPolicy(Permissions.Leave.Update, policy => policy.Requirements.Add(new PermissionRequirement(Permissions.Leave.Update)));

                options.AddPolicy(Permissions.Leave.Delete, policy => policy.Requirements.Add(new PermissionRequirement(Permissions.Leave.Delete)));

                options.AddPolicy(Permissions.Role.Create, policy => policy.Requirements.Add(new PermissionRequirement(Permissions.Role.Create)));
            });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return services;
        }
    }
}
