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
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public LeaveAllocationRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsWithDetails(int id)
        {
            var entity = await _dbContext.LeaveAllocations
                .Include(data => data.LeaveType)
                .FirstOrDefaultAsync(data => data.Id == id);
            return entity;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var entities = await _dbContext.LeaveAllocations
                .Include(data => data.LeaveType)
                .ToListAsync();
            return entities;
        }
    }
}
