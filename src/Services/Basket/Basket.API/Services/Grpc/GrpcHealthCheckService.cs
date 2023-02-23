
using Discount.Grpc.Protos;
using static Discount.Grpc.Protos.HealthCheckService;

namespace Basket.API.Services.Grpc
{
    public class GrpcHealthCheckService
    {
        private readonly HealthCheckServiceClient _healthCheckServiceClient;

        public GrpcHealthCheckService(HealthCheckServiceClient healthCheckServiceClient)
        {
            _healthCheckServiceClient = healthCheckServiceClient;
        }
        public async Task<PingReply> Ping(string Msg)
        {
            PingRequest request=new PingRequest { Msg = Msg };
            return  await _healthCheckServiceClient.PingAsync(request);
        }
    }
}
