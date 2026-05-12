using HR_LeaveManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces
{
    public interface IEmailJobService
    {
        void QueueLeaveRequestEmail(Email email);
    }
}
