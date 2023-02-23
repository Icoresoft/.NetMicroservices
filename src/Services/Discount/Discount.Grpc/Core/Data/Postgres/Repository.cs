using Core.Data;
using Core.Extensions;
using Dapper;
using Npgsql;
using System.Linq.Expressions;

namespace Core.Data.Postgres
{
    public class Repository<T,IdType> : IRepository<T,IdType> where T : IBaseEntity<IdType>
    {
        protected readonly NpgsqlConnection Connection;
        public Repository(NpgsqlConnection connection)
        {
            this.Connection = connection;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.QueryAsync<T>($"select * from {typeof(T).Name}");
        }
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> Filter)
        {
            throw new NotImplementedException();
        }
        public async Task<T> GetAsync(IdType Id)
        {
            //https://stackoverflow.com/questions/13291589/dapper-query-with-list-of-parameters
            return await Connection.QueryFirstOrDefaultAsync<T>($"select * from  {typeof(T).Name} where id=@id",new { id = Id } );
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> Filter)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CreateAsync(T item)
        {
            Dictionary<string,string> pairs= item.AsPropertyValuePair();

            string Query = $"insert into  {typeof(T).Name} ({pairs.AsQueryFields()}) values ({pairs.AsQueryValues()})";
            int effectedRows=await Connection.ExecuteAsync(Query);

            return effectedRows != 0;
        }
        public async Task<bool> UpdateAsync(T item)
        {
            Dictionary<string, string> pairs = item.AsPropertyValuePair();

            int effectedRows = await Connection.ExecuteAsync($"update   {typeof(T).Name} set {pairs.AsQueryKeyValuePair()} where id="+item.Id);

            return effectedRows != 0;
        }
        public async Task<bool> RemoveAsync(IdType Id)
        {
            int effectedRows= await Connection.ExecuteAsync($"delete from  {typeof(T).Name} where id=@id", new { id = Id });
            return effectedRows != 0;
        }
    }

}
