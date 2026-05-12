using HR_LeaveManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(Email email);
    }
}
