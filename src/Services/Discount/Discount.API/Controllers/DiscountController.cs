using Discount.API.Entities;
using Discount.API.Repositories;
using Discount.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly CouponService _couponService;
        public DiscountController(CouponService Service) {
            _couponService = Service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            var data =await _couponService.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync(Int32 Id)
        {
            var data = await _couponService.GetAsync(Id);
            return Ok(data);
        }
        [HttpGet("GetProductDiscount")]
        public async Task<IActionResult> GetProductDiscountAsync(string ProductCode)
        {
            var data = await _couponService.GetProductDiscountAsync(ProductCode);
            return Ok(data);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> CreateAsync(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _couponService.CreateAsync(coupon);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Coupon coupon)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _couponService.UpdateAsync(coupon);
            return Ok(data);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveAsync(Int32 Id)
        {
            var data = await _couponService.RemoveAsync(Id);
            return Ok(data);
        }
    }
}
