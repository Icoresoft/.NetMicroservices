using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class CouponProfile:Profile
    {
        public CouponProfile()
        {
            CreateMap<CouponModel,Coupon>().ReverseMap();
            CreateMap<Coupon,CouponModel>().ReverseMap(); 
        }
    }
}
