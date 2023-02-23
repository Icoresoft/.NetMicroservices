using Core.Data;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface ICouponRepository:IRepository<Coupon,Int32>
    {
        Task<Coupon> GetProductDiscountAsync(string ProductCode);
    }
}
