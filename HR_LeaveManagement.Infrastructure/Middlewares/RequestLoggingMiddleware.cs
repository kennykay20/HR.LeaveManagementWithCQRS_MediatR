using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next, 
            ILogger<RequestLoggingMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            var request = context.Request;

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation(
                "Incoming Request => {Method} {Path} | UserId: {UserId} | Email: {Email} | IP: {IP}",
                request.Method,
                request.Path,
                userId,
                email,
                context.Connection.RemoteIpAddress
            );

            await _next(context);

            stopWatch.Stop();

            _logger.LogInformation(
                "Outgoing Response => {StatusCode} completed in {ElaspedMiliseconds}ms",
                context.Response.StatusCode,
                stopWatch.ElapsedMilliseconds
             );
        }
    }
}
