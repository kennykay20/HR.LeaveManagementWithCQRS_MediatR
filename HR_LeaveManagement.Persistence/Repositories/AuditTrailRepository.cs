using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class AuditTrailRepository : GenericRepository<AuditTrail>, IAuditTrailRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public AuditTrailRepository(HRLeaveManagementDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AuditTrail>> GetAuditPageListAsync(int pageNumber, int pageSize)
        {

            var items = await _dbContext.AuditTrails
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }

        public async Task<int> GetTotalAuditCountAsync()
        {
            return await _dbContext.AuditTrails
                            .CountAsync();
        }
    }
}
