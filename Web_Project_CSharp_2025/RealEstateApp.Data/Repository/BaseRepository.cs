using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using RealEstateApp.Common;
using RealEstateApp.Data.Repository.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;

namespace RealEstateApp.Data.Repository
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {

        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public TEntity? GetById(TKey id)
        {
            TEntity? entity = dbSet.Find(id);
            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            TEntity? entity = await dbSet.FindAsync(id);
            return entity;
        }

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate)
        {
            TEntity? entity = this.dbSet.SingleOrDefault(predicate);
            return entity;
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity? entity = await this.dbSet.SingleOrDefaultAsync(predicate);
            return entity;
        }

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            TEntity? entity = this.dbSet.FirstOrDefault(predicate);
            return entity;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity? entity = await this.dbSet.FirstOrDefaultAsync(predicate);
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            TEntity[] entities = this.dbSet.ToArray();
            return entities;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            TEntity[] entities = await this.dbSet.ToArrayAsync();
            return entities;
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            IQueryable<TEntity>? entities = this.dbSet.AsQueryable();
            return entities;
        }

        public void Add(TEntity item)
        {
            this.dbSet.Add(item);
            this.context.SaveChanges();
        }

        public async Task AddAsync(TEntity item)
        {
            await this.dbSet.AddAsync(item);
            await this.context.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            this.dbSet.AddRange(items);
            this.context.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await this.dbSet.AddRangeAsync(items);
            await this.context.SaveChangesAsync();
        }

        // Soft Delete > IsDelete set to true >> using private methods to find the IsDeleted property for that entity 
        public bool Delete(TEntity entity)
        {
            this.PerformSoftDelete(entity);
            return this.Update(entity);
        }

        // Soft Delete async > IsDelete set to true >> using private methods to find the IsDeleted property for that entity 
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            this.PerformSoftDelete(entity);
            return await this.UpdateAsync(entity);
        }

        public bool HardDelete(TEntity entity)
        {
            this.dbSet.Remove(entity);
            int updateCount = this.context.SaveChanges();
            return updateCount > 0;
        }

        public async Task<bool> HardDeleteAsync(TEntity entity)
        {
            this.dbSet.Remove(entity);
            int updateCount = await this.context.SaveChangesAsync();
            return updateCount > 0;
        }

        public bool Update(TEntity item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbSet.Entry(item).State = EntityState.Modified;
                this.context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            try
            {
                this.dbSet.Attach(item);
                this.dbSet.Entry(item).State = EntityState.Modified;
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Any()
        {
            return this.dbSet.Any();
        }

        public Task<bool> AnyAsync()
        {
            return this.dbSet.AnyAsync();
        }

        public async Task ExecuteInTransactionAsync(TEntity entity) 
        { 
           using var transaction = await this.context.Database.BeginTransactionAsync();

            try
            {
                await this.dbSet.AddAsync(entity);
                await this.context.SaveChangesAsync();  
                await transaction.CommitAsync();   
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException(String.Format(ExceptionMessages.DatabaseTransactionFailed, ex.Message));
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        // Below private methods are using Reflection to check if the properties which we are trying to delete has IsDelete property and if YES it is set to true;
        // Used for the Delete methods.
        private void PerformSoftDelete(TEntity entity)
        {
            PropertyInfo? isDeletedProperty = this.GetIsDeletedProperty(entity);

            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SoftDelete);
            }

            isDeletedProperty.SetValue(entity, true);
        }

        private PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(x => x.PropertyType == typeof(bool) && x.Name == AppConstants.IsDeletedPropertyName);
        }
    }
}
