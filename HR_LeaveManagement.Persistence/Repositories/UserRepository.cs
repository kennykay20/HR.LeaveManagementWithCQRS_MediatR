using HR_LeaveManagement.Application.Contracts.Persistence;
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public UserRepository(HRLeaveManagementDbContext dbContext) 
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var result = await _dbContext.Users.Where((user) => user.Email == email && user.IsDeleted == false).FirstOrDefaultAsync();
            return result ?? null;
        }

        public async Task<User?> GetUserById(int userId)
        {
            var result = await _dbContext.Users.Where((user) => user.Id == userId && user.IsDeleted == false).FirstOrDefaultAsync();
            return result ?? null;
        }

        public async Task<List<User>> GetUserPageListAsync(int pageNumber, int pageSize)
        {
            var items = await _dbContext.Users.Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _dbContext.Users
                .Where(x => !x.IsDeleted)
                .CountAsync();
        }

        public async Task<User?> GetUserRolesByUserId(int userId)
        {
            var user = await _dbContext.Users.Include(user => user.UserRoles)
                            .FirstOrDefaultAsync(user => user.Id == userId && user.IsDeleted == false);

            return user ?? null;
        }
    }
}
