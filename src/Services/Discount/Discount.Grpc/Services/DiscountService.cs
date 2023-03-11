using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static Discount.Grpc.Protos.DiscountService;

namespace Discount.Grpc.Services
{
    public class DiscountService:DiscountServiceBase
    {
        private readonly ICouponRepository _CouponRepository;

        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _CouponRepository = couponRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public override async Task<CouponModel> GetProductDiscount(ProductCodeModel request, ServerCallContext context)
        {
            var Coupon = await _CouponRepository.GetProductDiscountAsync(request.ProductCode);
            if(Coupon != null)
            {
                return _mapper.Map<CouponModel>(Coupon);
               
            }
            var EmptyCoupon = new CouponModel();
            return EmptyCoupon;
        }
        //public override async Task<RepeatedCouponModel> GetAll(Empty request, ServerCallContext context)
        //{
        //    IEnumerable<Coupon> items= await _CouponRepository.GetAllAsync();
        //    return items;
        //}
        public override async Task<ResultModel> Create(CouponModel request, ServerCallContext context)
        {
            var coupon=_mapper.Map<Coupon>( request);
            var IsSuccess=await _CouponRepository.CreateAsync(coupon);
            return new ResultModel { IsSuccess = IsSuccess };
        }
        public override async Task<ResultModel> Update(CouponModel request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request);
            var IsSuccess = await _CouponRepository.UpdateAsync(coupon);
            return new ResultModel { IsSuccess = IsSuccess };
        }
        public override async Task<ResultModel> Remove(ProductIdModel request, ServerCallContext context)
        {
            var IsSuccess = await _CouponRepository.RemoveAsync(ProductIdModel.IdFieldNumber);
            return new ResultModel { IsSuccess = IsSuccess };
        }
    }
}
