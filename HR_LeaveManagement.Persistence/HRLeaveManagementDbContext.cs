using HR_LeaveManagement.Domain.Common;
using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence
{
    public class HRLeaveManagementDbContext : DbContext
    {
        public HRLeaveManagementDbContext(DbContextOptions<HRLeaveManagementDbContext> options) 
            : base(options)
        {
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<Policy> Policies { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

        // this automatically registers all configurations.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRLeaveManagementDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseCommonEntity>())
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
                Console.WriteLine("Entity state = ", entry.State);
                if(entry.State == EntityState.Added)
                {
                    Console.WriteLine("Inside entry state for add");
                    entry.Entity.DateCreated = DateTime.Now;
                    entry.Entity.IsDeleted = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
