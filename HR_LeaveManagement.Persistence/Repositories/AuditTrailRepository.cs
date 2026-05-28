using HR_LeaveManagement.Application.Contracts.Persistences;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class AuditTrailRepository : GenericRepository<AuditTrail>, IAuditTrailRepository
    {
        private readonly HRLeaveManagementDbContext _context;

        public AuditTrailRepository(HRLeaveManagementDbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
