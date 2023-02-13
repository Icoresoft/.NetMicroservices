using Basket.API.Core.Data.Entity;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json;

namespace Core.Data.Redis
{
    public class Repository<T, IdType> : IRepository<T, IdType> where T : IBaseEntity<IdType>
    {
        protected readonly IDistributedCache Cache;

        public Repository(IDistributedCache Cache)
        {
            this.Cache = Cache;
        }
        public async Task<ICollection<T>> GetAllAsync()
        {
            throw new NotSupportedException();
        }
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> Filter)
        {
            throw new NotSupportedException();
        }
        public async Task<T> GetAsync(IdType Id)
        {
            var data = await Cache.GetAsync(Convert.ToString(Id));
            return JsonSerializer.Deserialize<T>(data);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> Filter)
        {
            throw new NotSupportedException();
        }
        public async Task CreateAsync(T item)
        {
            await AddOrUpdate(item);
        }
        public async Task<bool> UpdateAsync(T item)
        {
            return await AddOrUpdate(item);
        }
        public async Task<bool> RemoveAsync([Required] IdType Id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                throw new ArgumentNullException(nameof(Id));
            }
            try
            {
                await Cache.RemoveAsync(Convert.ToString(Id));
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> AddOrUpdate(T item)
        {
            if (string.IsNullOrEmpty(Convert.ToString(item.Id)))
            {
                throw new ArgumentNullException(nameof(item.Id));
            }
            var key = Convert.ToString(item.Id);
            try
            {
                var Data = JsonSerializer.Serialize(item);
                await Cache.SetStringAsync(key, Data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
