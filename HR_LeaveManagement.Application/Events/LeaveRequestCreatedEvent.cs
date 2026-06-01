using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Events
{
    public record LeaveRequestCreatedEvent(
        int LeaveRequestId, 
        string Email, 
        int LeaveTypeId,
        DateTime StartDate,
        DateTime EndDate,
        string? RequestComments
        );
}
