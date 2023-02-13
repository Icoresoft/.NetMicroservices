using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Caching
{
    //---------------------------------------------------------------------------------------------
    public class Cachable
    {
        public string Key { get; set; }
        public Cachable(string Key)
        {
            this.Key = Key;
        }
    }
    //---------------------------------------------------------------------------------------------
    public enum CacheType { InMemory = 0, Redis = 1 }
    //---------------------------------------------------------------------------------------------
    //todo: Abstract class ,Options for each Provider
    public class CacheOptions
    {
        //todo: Default Options Paramas like reading connection string
        public int DefaultCachingPeriod { get; set; } = 60;
        public String ConnectionStr { get; set; } = string.Empty;
        //important in case if you are using redis for more than one app
        //e.g. CT_ will be a prefix to key name
        public String InstanceName { get; set; } = string.Empty;
        public CacheType CacheType { get; set; } = Core.Caching.CacheType.InMemory;
    }
    //---------------------------------------------------------------------------------------------
    public static class Extensions
    {
        public static IServiceCollection AddCache(this IServiceCollection Services, Action<CacheOptions> Options)
        {
            CacheOptions Ops = new CacheOptions();
            Options.Invoke(Ops);

            if (Ops.CacheType == Core.Caching.CacheType.Redis)
            {
                if (string.IsNullOrEmpty(Ops.ConnectionStr))
                {
                    throw new ArgumentNullException(Ops.ConnectionStr);
                }
                //this will add  object of type  DistributedCache to the container 
                Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = Ops.ConnectionStr;
                    //important in case if you are using redis for more than one app
                    //e.g. CT_ will be a prefix to key name
                    options.InstanceName = Ops.InstanceName;
                });

                Services.AddSingleton<ICache, RedisCache>();
                //Services.AddSingleton<ICache>(ServiceProvider =>
                //{

                //    var DistributedCache = ServiceProvider.GetService<IDistributedCache>();
                //    return new Core.Caching.RedisCache(DistributedCache, Ops.DefaultCachingPeriod);
                //});
            }
            else if (Ops.CacheType == Core.Caching.CacheType.InMemory)
            {
                Services.AddSingleton<ICache>(new MemoryCache(Ops.DefaultCachingPeriod));
                //Nothing
            }
            return Services;
        }
    }
    //\////////////////////////////////////////////////////////////////////////////////////////////
    public interface ICache
    {
        int DefaultCachingPeriod { get; set; }
        bool IsKeyExisting(string Key);
        Task<T> GetAsync<T>(string Key);
        Task SetAsync<T>(string Key, T Value, int CachingPeriod = 0);
        Task RemoveAsync(string Key);
    }
    //\////////////////////////////////////////////////////////////////////////////////////////////
    public class MemoryCache : ICache
    {
        public int DefaultCachingPeriod { get; set; } = 0;

        //-----------------------------------------------------------------------------------------
        public MemoryCache(int DefaultCachingPeriod)
        {
            this.DefaultCachingPeriod = DefaultCachingPeriod;
        }
        //-----------------------------------------------------------------------------------------
        public bool IsKeyExisting(string Key)
        {
            return System.Runtime.Caching.MemoryCache.Default.Contains(Key);
        }
        //-----------------------------------------------------------------------------------------
        public async Task<T> GetAsync<T>(string Key)
        {
            var Data = System.Runtime.Caching.MemoryCache.Default.Get(Key);
            if (Data is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(Convert.ToString(Data));
        }
        //-----------------------------------------------------------------------------------------
        public async Task SetAsync<T>(string Key, T Value, int CachingPeriod = 0)
        {
            CachingPeriod = CachingPeriod != 0 ? CachingPeriod : DefaultCachingPeriod;
            var Data = JsonSerializer.Serialize<T>(Value);
            System.Runtime.Caching.MemoryCache.Default.Set(Key, Data, DateTimeOffset.Now.AddSeconds(CachingPeriod));
        }
        //-----------------------------------------------------------------------------------------
        public async Task RemoveAsync(string Key)
        {
            System.Runtime.Caching.MemoryCache.Default.Remove(Key);
        }
        //-----------------------------------------------------------------------------------------
    }
    //\////////////////////////////////////////////////////////////////////////////////////////////
    public class RedisCache : ICache
    {
        //-----------------------------------------------------------------------------------------
        public int DefaultCachingPeriod { get; set; } = (60*60*24*30);
        private readonly IDistributedCache Cache;
        //-----------------------------------------------------------------------------------------
        public RedisCache(IDistributedCache Cache)/*, int DefaultCachingPeriod)*/
        {
            this.Cache = Cache;
           // this.DefaultCachingPeriod = 60000; //DefaultCachingPeriod;
        }
        //-----------------------------------------------------------------------------------------
        public bool IsKeyExisting(string Key)
        {
            return Cache.Get(Key) != null;
        }
        //-----------------------------------------------------------------------------------------
        public async Task<T> GetAsync<T>(string Key)
        {
            var Data = await Cache.GetStringAsync(Key);
            if (Data is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(Data);
        }
        //-----------------------------------------------------------------------------------------
        public async Task SetAsync<T>(string Key, T Value, int CachingPeriod = 0)
        {
            CachingPeriod = CachingPeriod != 0 ? CachingPeriod : DefaultCachingPeriod;

            DistributedCacheEntryOptions Options = new DistributedCacheEntryOptions();
            Options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(CachingPeriod);

            var Data = JsonSerializer.Serialize<T>(Value);
            await Cache.SetStringAsync(Key, Data, Options);
        }
        //-----------------------------------------------------------------------------------------
        public async Task RemoveAsync(string Key)
        {
            await Cache.RemoveAsync(Key);
        }
        //-----------------------------------------------------------------------------------------
    }
    //\////////////////////////////////////////////////////////////////////////////////////////////
    //---------------------------------------------------------------------------------------------
}
