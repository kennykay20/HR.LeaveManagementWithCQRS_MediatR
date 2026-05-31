using Hangfire;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Infrastructure.Services;
using HR_LeaveManagement.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailJobService, EmailJobService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<ICacheService, CacheService>();

            services.AddHangfire(config =>
                    config.UseSqlServerStorage(
                        configuration.GetConnectionString("HRLeaveConnectionString")
                    ));

            services.AddHangfireServer();

            return services;
        }
    }
}
