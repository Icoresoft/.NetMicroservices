using Core.Data.Postgres;
using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class CouponRepository: Repository<Coupon,Int32> , ICouponRepository
    {
        private readonly NpgsqlConnection _connection;
        public CouponRepository(NpgsqlConnection connection):base(connection) 
        {
            _connection= connection;
        }
        public async Task<Coupon> GetProductDiscountAsync(string ProductCode)
        {
            return await _connection.QueryFirstOrDefaultAsync<Coupon>("select * from coupon where ProductCode=@PCode", new { @PCode = ProductCode });
        }
    }
}
