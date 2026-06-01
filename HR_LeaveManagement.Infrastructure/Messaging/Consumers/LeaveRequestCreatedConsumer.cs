using HR_LeaveManagement.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Infrastructure.Messaging.Consumers
{
    public class LeaveRequestCreatedConsumer 
        : IConsumer<LeaveRequestCreatedEvent>
    {
        private readonly ILogger<LeaveRequestCreatedConsumer> _logger;

        public LeaveRequestCreatedConsumer(
            ILogger<LeaveRequestCreatedConsumer> logger
            )
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<LeaveRequestCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Leave Request Created - Id: {id}, Email: {email}",
                message.LeaveRequestId,
                message.Email);

            return Task.CompletedTask;
        }
    }
}
