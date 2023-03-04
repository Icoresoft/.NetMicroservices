using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;

namespace Ordering.Infrastructure.Repository
{
    public static class OrderExtensions
    {
        //it depends on DbSet which should be accessed through repo only 
        public static async Task<IReadOnlyList<Order>> GetByUserNameExtAsync1(this DbSet<Order> set, string UserName)
        {
            return await set.Where(o => o.UserName == UserName).ToListAsync();
        }
        //it depends on Repo but context must be public 
        public static async Task<IReadOnlyList<Order>> GetByUserNameExtAsync2(this Repository<Order, Int32> Repo, string UserName)
        {
            return await Repo._dbContext.Set<Order>().Where(o => o.UserName == UserName).ToListAsync();
        }
        //best case: no access to context + extend repo BUT no Abtraction to dpends on 
        public static async Task<IReadOnlyList<Order>> GetByUserNameExtAsync3(this Repository<Order, Int32> Repo, string UserName)
        {
            return await Repo.GetSet().Where(o => o.UserName == UserName).ToListAsync();
        }
        public static async Task<IReadOnlyList<Order>> GetByUserNameExtAsync4(this Application.Contracts.Persistance.IRepository<Order, Int32> Repo, string UserName)
        {
            var data = await Repo.GetByQuery(o => o.UserName == UserName).ToListAsync();
            return data;
        }
    }
}