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
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public LeaveRequestRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? approvalStatus)
        {
            leaveRequest.Approved = approvalStatus;
            await Update(leaveRequest);
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var entity = await _dbContext.LeaveRequests
                .Include(data => data.LeaveType)
                .FirstOrDefaultAsync(data => data.Id == id);
            return entity;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails()
        {
            var entities = await _dbContext.LeaveRequests
                .Include(data => data.LeaveType)
                .ToListAsync();
            return entities;
        }
    }
}
