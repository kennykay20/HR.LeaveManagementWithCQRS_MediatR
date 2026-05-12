using Hangfire;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Infrastructure.Services
{
    public class EmailJobService : IEmailJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailSender _emailSender;

        public EmailJobService(IBackgroundJobClient backgroundJobClient, IEmailSender emailSender)
        {
            _backgroundJobClient = backgroundJobClient;
            _emailSender = emailSender;
        }
        public void QueueLeaveRequestEmail(Email email)
        {
            _backgroundJobClient.Enqueue(() => _emailSender.SendEmail(email));
        }
    }
}
