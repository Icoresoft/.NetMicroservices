using Discount.Grpc.Protos;
using static Discount.Grpc.Protos.DiscountService;

namespace Basket.API.Services.Grpc
{
    public class GrpcDiscountService
    {
        private readonly DiscountServiceClient _DiscountServiceClient;
        public GrpcDiscountService(DiscountServiceClient discountServiceClient)
        {
            _DiscountServiceClient = discountServiceClient;
        }
        public async Task<CouponModel> GetProductDiscountAsync(string ProductCode)
        {
            ProductCodeModel model=new ProductCodeModel { ProductCode = ProductCode };
            var coupon=await _DiscountServiceClient.GetProductDiscountAsync(model);
            return coupon;
        }
    }
}
