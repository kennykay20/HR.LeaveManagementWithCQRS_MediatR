using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure
{
    public class HangfireCustomBasicAuthenticationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _username;
        private readonly string _password;

        public HangfireCustomBasicAuthenticationFilter(IConfiguration configuration)
        {
            // Load credentials (env first, fallback to appsettings)
            _username = Environment.GetEnvironmentVariable("HANGFIRE_USERNAME")
                        ?? configuration["HangfireDashboard:Username"];

            // Load credentials (env first, fallback to appsettings)
            _password = Environment.GetEnvironmentVariable("HANGFIRE_PASSWORD")
                        ?? configuration["HangfireDashboard:Password"];
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return true;
        }

        private void Challenge(HttpContext context)
        {

        }
    }
}
