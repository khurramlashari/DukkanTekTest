using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {

       Task<TEntity> GetAsync(int id);
        Task<List<TEntity>> GetAllAsync(bool IsNoTracking = true);
        IQueryable<TEntity> GetAllQueryable();
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = false);
        IQueryable<TEntity> FindQueryable();
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void UpdateRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
         
    }
}
