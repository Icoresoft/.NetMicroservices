using Core.Data;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface ICouponRepository:IRepository<Coupon,Int32>
    {
        Task<Coupon> GetProductDiscountAsync(string ProductCode);
    }
}
