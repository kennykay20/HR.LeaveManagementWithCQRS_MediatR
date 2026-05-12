using HR_LeaveManagement.Application.Persistence.Contracts;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public LeaveTypeRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
