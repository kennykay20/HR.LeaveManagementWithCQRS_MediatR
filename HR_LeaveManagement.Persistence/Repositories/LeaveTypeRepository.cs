using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<LeaveType> GetLeaveTypeByName(string name)
        {
            var entity = await _dbContext.LeaveTypes.FirstOrDefaultAsync(data => data.Name.ToLower() == name.ToLower() && data.IsDeleted == false);
            return entity;
        }
    }
}
