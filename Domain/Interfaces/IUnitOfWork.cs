using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> BaseRepositoryAsync<T>() where T : class;
        Task<bool> SaveChangesAsync(bool overwriteDbChangesInCaseOfConcurrentUpdates = true);
        void DiscardChanges();
    }
}
