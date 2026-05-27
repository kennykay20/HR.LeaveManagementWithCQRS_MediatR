using HR_LeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HRLeaveManagementDbContext _dbContext;

        public GenericRepository(HRLeaveManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Exist(int id)
        {
            var entity = await Get(id);
            return entity != null;
        }

        public async Task<T> Get(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            var entities = await _dbContext.Set<T>().ToListAsync();
            return entities;
        }

        public async Task Update(T entity)
        {
            //_dbContext.Set<T>().Update(entity);
            //_dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            //_dbContext.Set<T>().Remove(entity);
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
