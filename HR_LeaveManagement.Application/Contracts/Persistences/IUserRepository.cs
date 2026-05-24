using HR_LeaveManagement.Application.Contracts.Persistence;
using HR_LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Application.Contracts.Persistences
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetUserPageListAsync(int pageNumber, int pageSize);
    }
}
