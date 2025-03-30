using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data.Repository.Contracts;

namespace RealEstateApp.Data.Repository
{
    public class BaseRepository<TType, TId> : IRepository<TType, TId>
        where TType : class
    {

        private readonly ApplicationDbContext context;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TType>();
        }

        public void Add(TType item)
        {
            this.dbSet.Add(item);   
            this.context.SaveChanges(); 
        }

        public async Task AddAsync(TType item)
        {
            await this.dbSet.AddAsync(item);
            await this.context.SaveChangesAsync();
        }

        public bool Delete(TId id)
        {
            TType entity = this.GetById(id);

            if (entity == null)
            {
                return false;
            }

            this.dbSet.Remove(entity);
            this.context.SaveChanges();
            return true;    
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            TType entity = await this.GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync();
            return true;
        }

        public IEnumerable<TType> GetAll()
        {
            return this.dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await this.dbSet.ToArrayAsync();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }

        public TType GetById(TId id)
        {
            TType entity = dbSet.Find(id);
            return entity; 
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await dbSet.FindAsync(id);
            return entity;
        }

        public bool Update(TType item)
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

        public async Task<bool> UpdateAsync(TType item)
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
    }
}
