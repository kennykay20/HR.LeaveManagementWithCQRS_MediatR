using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        // add methods that is specific to this repository only here
        Task<LeaveType> GetLeaveTypeByName(string name);
    }
}
