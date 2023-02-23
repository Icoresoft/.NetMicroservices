using Core.Data;

namespace Discount.Grpc.Entities
{
    public class Coupon:BaseEntity<Int32>
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
