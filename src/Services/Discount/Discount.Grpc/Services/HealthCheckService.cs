using Discount.Grpc.Protos;
using Grpc.Core;
using static Discount.Grpc.Protos.HealthCheckService;

namespace Discount.Grpc.Services
{
    public class HealthCheckService:HealthCheckServiceBase
    {
        public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
        {
            PingReply pingReply = new PingReply { ServerDateTime = $"Welcome: {request.Msg},Current Date: {DateTime.Now}" };
            return pingReply;
        }
    }
}
