using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order, Int32>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            //1:Break Pattern  you can access context
            //return await _dbContext.orders.GetByUserNameExtAsync1(userName);
            //2:
            return await this.GetByUserNameExtAsync2(userName);
            //return await _dbContext.orders.Where(o=>o.UserName==userName).ToListAsync();
            //return this.GetByUserNameExtAsync4(userName);
        }
    }
}
