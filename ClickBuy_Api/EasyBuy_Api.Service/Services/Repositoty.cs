using System.Linq.Expressions;
using ClickBuy_Api.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace ClickBuy_Api.Service.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ClickBuyDbContext _db;
        protected DbSet<T> dbSet;

        public Repository(ClickBuyDbContext db)
        {
            _db = db;
            dbSet = db.Set<T>();
        }

        public async Task Add(T entity) => await dbSet.AddAsync(entity);

        public async Task AddRange(List<T> list) => await dbSet.AddRangeAsync(list);

        public IQueryable<T> AsQueryable() => dbSet.AsNoTracking();

        public void Delete(T entity, bool? isActive)
        {
            if (isActive == true)
            {
                entity.GetType().GetProperty("IsActive")?.SetValue(entity, true);
            }
            else
            {
                dbSet.Remove(entity);
            }
        }

        public void DeleteRange(List<T> entities, bool? isActive)
        {
            if (isActive == true)
            {
                entities.ForEach(e => e.GetType().GetProperty("IsActive")?.SetValue(e, true));
            }
            else
            {
                dbSet.RemoveRange(entities);
            }
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool asNoTracking = true)
        => asNoTracking ? dbSet.Where(expression).AsNoTracking() : dbSet.Where(expression);

        public async Task<T> GetByIdAsync(Guid id)
        => await dbSet.FindAsync(id);
        public IQueryable<T> ToPagedList(int pageNumber, int pageSize)
        => dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking();

        public bool Update(T entity)
        {
            var entry = _db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
            return true;
        }

        public bool UpdateRange(List<T> entities)
        {
           var entry = _db.Entry(entities);
            if (entry.State == EntityState.Detached)
            {
                dbSet.AttachRange(entities);
            }
            entry.State = EntityState.Modified;
            return true;
        }
    }
}
