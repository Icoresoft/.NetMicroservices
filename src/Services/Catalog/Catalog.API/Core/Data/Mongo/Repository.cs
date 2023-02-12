using Core.Data;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Data.Mongo
{
    public class Repository<T,IdType> : IRepository<T,IdType> where T : IBaseEntity<IdType>
    {
        protected readonly IMongoCollection<T> dbCollection;
        protected readonly FilterDefinitionBuilder<T> filter = Builders<T>.Filter;
        public Repository(IMongoCollection<T> dbCollection)
        {
            this.dbCollection = dbCollection;
        }
        public async Task<ICollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filter.Empty).ToListAsync();
        }
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> Filter)
        {
            return await dbCollection.Find(Filter).ToListAsync();
        }
        public async Task<T> GetAsync(IdType Id)
        {
            return await dbCollection.Find(filter.Eq(obj => obj.Id, Id)).SingleOrDefaultAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> Filter)
        {
            return await dbCollection.Find(Filter).SingleOrDefaultAsync();
        }
        public async Task CreateAsync(T item)
        {
            await dbCollection.InsertOneAsync(item);
        }
        public async Task<bool> UpdateAsync(T item)
        {
            var result=await dbCollection.ReplaceOneAsync(filter.Eq(obj => obj.Id, item.Id), item);
           return result.IsAcknowledged && result.ModifiedCount> 0;
        }
        public async Task<bool> RemoveAsync(IdType Id)
        {
            var result=await dbCollection.DeleteOneAsync(filter.Eq(e => e.Id, Id));
            return result.IsAcknowledged && result.DeletedCount> 0;
        }
    }

}
