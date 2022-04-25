using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
         
        public UnitOfWork(ApplicationDbContext context)
        {
            
            _applicationDbContext = context;
        }
        public void DiscardChanges()
        {
            foreach (var Entry in _applicationDbContext.ChangeTracker.Entries())
            {
                Entry.State = EntityState.Unchanged;
            }
        }
        public IRepository<T> BaseRepositoryAsync<T>() where T : class
        {
            return new BaseRepository<T>(_applicationDbContext);
        }

        public async Task<bool> SaveChangesAsync(bool overwriteDbChangesInCaseOfConcurrentUpdates = true)
        {
            bool saveFailed = false;
            do
            {
                saveFailed = false;

                try
                {
                    int count = await _applicationDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    if (overwriteDbChangesInCaseOfConcurrentUpdates)
                    {
                        foreach (var Entry in ex.Entries)
                        {
                            foreach (var property in Entry.Entity.GetType().GetTypeInfo().DeclaredProperties)
                            {
                                //var originalValue = Entry.Property(property.Name).OriginalValue;
                                var currentValue = Entry.Property(property.Name).CurrentValue;
                                Entry.Property(property.Name).OriginalValue = currentValue;
                            }
                        }
                        saveFailed = false;

                    }
                    else
                    {
                        foreach (var Entry in ex.Entries)
                        {
                            foreach (var property in Entry.Entity.GetType().GetTypeInfo().DeclaredProperties)
                            {
                                var originalValue = Entry.Property(property.Name).OriginalValue;
                                //var currentValue = Entry.Property(property.Name).CurrentValue;
                                Entry.Property(property.Name).CurrentValue = originalValue;
                            }
                        }
                        saveFailed = false;
                    }
                }
            } while (saveFailed);
            return await Task.FromResult(saveFailed);
        }
    }
}
