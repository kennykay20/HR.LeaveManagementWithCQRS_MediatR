using HR_LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Application.Persistence.Contracts
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        // add methods that is specific to this repository only here
    }
}
