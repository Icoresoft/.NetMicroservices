using Discount.API.Entities;
using Discount.API.Repositories;

namespace Discount.API.Services
{
    public class CouponService
    {
        private readonly  ICouponRepository _couponRepository;
        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        public async Task<Coupon> GetProductDiscountAsync(string ProductCode)
        {
            return await _couponRepository.GetProductDiscountAsync(ProductCode);
        }
        public async Task<Coupon> GetAsync(Int32 Id)
        {
            return await _couponRepository.GetAsync(Id);
        }
        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _couponRepository.GetAllAsync();
        }
        public async Task<bool> CreateAsync(Coupon coupon)
        {
            return await _couponRepository.CreateAsync(coupon);
        }

        public async Task<bool> UpdateAsync(Coupon coupon)
        {
            return await _couponRepository.UpdateAsync(coupon);
        }

        public async Task<bool> RemoveAsync(Int32 Id)
        {
            return await _couponRepository.RemoveAsync(Id);
        }
    }
}
