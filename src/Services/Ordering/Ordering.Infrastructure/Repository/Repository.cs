using Core.Data;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Infrastructure.Persistance;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Infrastructure.Repository
{
    public class Repository<T, IdT> : IRepository<T, IdT> where T : EntityBase<IdT>
    {
        public readonly OrderContext _dbContext;
        public Repository(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
        public DbSet<T> GetSet()
        {
            return _dbContext.Set<T>();
        }
        public IQueryable<T> GetByQuery(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> Query = _dbContext.Set<T>();
            Query=predicate==null ? Query : Query.Where(predicate);
            return Query.AsQueryable();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity; //note that you must commit to get ID
        }

        public async Task UpdateAsync(T entity)
        {
            //use =>_dbContext.Entry(entity).State = EntityState.Modified;
            //Or 
            _dbContext.Set<T>().Update(entity);
        }
        public async Task DeleteAsync(IdT id)
        {
            var entity=await GetByIdAsync(id);
             _dbContext.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            //as query object
            IQueryable<T> Query = _dbContext.Set<T>();
            //tracking
            Query = disableTracking ? Query : Query.AsNoTracking();
            //includes
            Query=string.IsNullOrWhiteSpace(includeString)?Query:Query.Include(includeString);
            //predicate
            Query=predicate==null?Query:Query.Where(predicate);

            //order and return 
            return orderBy != null ? await orderBy(Query).ToListAsync() : await Query.ToListAsync();

        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            //as query object
            IQueryable<T> Query = _dbContext.Set<T>();
            //tracking
            Query = disableTracking ? Query : Query.AsNoTracking();

            //includes
            if (includes != null)
                Query = includes.Aggregate(Query, (current, include) => current.Include(include));
           
            //predicate
            Query = predicate == null ? Query : Query.Where(predicate);

            //order and return 
            return orderBy != null ? await orderBy(Query).ToListAsync() : await Query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(IdT Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

    }
}
