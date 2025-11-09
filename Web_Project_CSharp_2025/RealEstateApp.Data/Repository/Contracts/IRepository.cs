
using System.Linq.Expressions;

namespace RealEstateApp.Data.Repository.Contracts
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity? GetById(TKey id);

        Task<TEntity?> GetByIdAsync(TKey id);

        TEntity? SingleOrDefault(Func<TEntity, bool> predicate);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity? FirstOrDefault(Func<TEntity, bool> predicate);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetAllAttached();

        void Add(TEntity item);

        Task AddAsync(TEntity item);

        void AddRange(IEnumerable<TEntity> items);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        // Soft Delete
        bool Delete(TEntity entity);

        // Soft Delete async
        Task<bool> DeleteAsync(TEntity entity);

        bool HardDelete(TEntity entity);

        Task<bool> HardDeleteAsync(TEntity entity);
        bool Any();

        Task<bool> AnyAsync();

        Task ExecuteInTransactionAsync(TEntity entity);

        bool Update(TEntity item);

        Task<bool> UpdateAsync(TEntity item);
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
