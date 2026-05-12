using Hangfire;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailJobService, EmailJobService>();

            services.AddHangfire(config =>
                    config.UseSqlServerStorage(
                        configuration.GetConnectionString("HRLeaveConnectionString")
                    ));

            services.AddHangfireServer();

            return services;
        }
    }
}
