using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
       
        protected readonly ApplicationDbContext _context;

      

        public BaseRepository(ApplicationDbContext context )
        {
            _context = context;
             
        }


        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = false)
        {
            if (IsNoTracking)
            {
                return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            return await _context.Set<TEntity>().FindAsync(predicate);
        }


        public IQueryable<TEntity> FindQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<TEntity>> GetAllAsync(bool IsNoTracking = true)
        {
            if (IsNoTracking)
            {
                return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().LastOrDefaultAsync(predicate);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
        public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).CountAsync();
        }
        public void UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }

        


        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
         
    }
}
