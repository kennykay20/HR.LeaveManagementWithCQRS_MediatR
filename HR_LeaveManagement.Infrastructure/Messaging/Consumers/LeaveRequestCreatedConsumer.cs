using HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces;
using HR_LeaveManagement.Application.Contracts.Infrastructure.Templates;
using HR_LeaveManagement.Application.Events;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Domain.Entities;
using HR_LeaveManagement.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace HR_LeaveManagement.Infrastructure.Messaging.Consumers
{
    public class LeaveRequestCreatedConsumer 
        : IConsumer<LeaveRequestCreatedEvent>
    {
        private readonly ILogger<LeaveRequestCreatedConsumer> _logger;
        private readonly IEmailSender _emailSender;
        private readonly HRLeaveManagementDbContext _dbContext;

        public LeaveRequestCreatedConsumer(
            ILogger<LeaveRequestCreatedConsumer> logger,
            IEmailSender emailSender,
            HRLeaveManagementDbContext dbContext)
        {
            _logger = logger;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<LeaveRequestCreatedEvent> context)
        {
            _logger.LogInformation("Consumer started. ");

            
            var message = context.Message;
            var messageId = context.Message.LeaveRequestId.ToString();

            if (messageId == null)
                return;

            var processed = await _dbContext.ProcessedMessages.AnyAsync(x => x.MessageId == messageId);

            if (processed)
            {
                _logger.LogWarning(
                    "Duplicate message ignored {MessageId}",
                    messageId);

                return;
            }

            // send email
            var email = new Email
            {
                To = message.Email ?? "kennyoluwadamilare20@gmail.com",
                Subject = "Leave Request Submitted",
                Body = EmailTemplateGetter.LeaveRequestNotification(message.StartDate, message.EndDate)
            };

            await _emailSender.SendEmail(email);

            _logger.LogInformation(
                    "Email sent for leave request {id}",
                    message.LeaveRequestId);

            _logger.LogInformation("Consumer ended. ");

            _dbContext.ProcessedMessages.Add(
                    new ProcessedMessage
                    {
                        MessageId = messageId,
                        ProcessedAt = DateTime.UtcNow
                    });

            await _dbContext.SaveChangesAsync();

            throw new Exception("Testing RabbitMQ - an error occur");
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"An error occur - {ex.Message}");
            //    throw new Exception($"Testing RabbitMQ - {ex.Message}");
            //}

            //return Task.CompletedTask;
        }
    }
}
